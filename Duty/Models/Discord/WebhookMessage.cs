using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RestoreMonarchy.Duty.Helpers;
using Rocket.Core.Logging;

namespace RestoreMonarchy.Duty.Models.Discord;

 public class WebhookMessage : ICloneable
    {
        [JsonIgnore]
        [XmlAttribute]
        public bool Enabled { get; set; }
        [JsonIgnore]
        public string WebhookUrl { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        public bool ShouldSerializeUsername() => !string.IsNullOrEmpty(Username);
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        public bool ShouldSerializeAvatarUrl() => !string.IsNullOrEmpty(AvatarUrl);
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonIgnore]
        [XmlArrayItem("ContentLine")]
        public string[] ContentLines { get; set; }
        public bool ShouldSerializeContent() => !string.IsNullOrEmpty(Content);
        [JsonProperty("embeds")]
        public Embed[] Embeds { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public byte[][] Files { get; set; }
        
        public void Prepare()
        {
            if (ContentLines != null && ContentLines.Length > 0)
            {
                StringBuilder sb = new();
                foreach (string line in ContentLines)
                {
                    sb.AppendLine(line);
                }
                Content = sb.ToString();
            }

            foreach (Embed embed in Embeds ?? [])
            {
                embed.Prepare();
            }
        }

        public void Validate()
        {
            if (ValidationHelper.IsUrlInvalid(AvatarUrl))
            {
                Logger.LogWarning($"Invalid AvatarUrl in WebhookMessage: {AvatarUrl}");
                AvatarUrl = null;                
            }

            foreach (Embed embed in Embeds ?? [])
            {
                if (ValidationHelper.IsUrlInvalid(embed.Author?.IconUrl ?? null))
                {
                    Logger.LogWarning($"Invalid Author IconUrl in WebhookMessage: {embed.Author?.IconUrl}");
                    embed.Author.IconUrl = null;
                }

                if (ValidationHelper.IsUrlInvalid(embed.Footer?.IconUrl ?? null))
                {
                    Logger.LogWarning($"Invalid Footer IconUrl in WebhookMessage: {embed.Footer?.IconUrl}");
                    embed.Footer.IconUrl = null;
                }

                if (ValidationHelper.IsUrlInvalid(embed.Image?.Url ?? null))
                {
                    Logger.LogWarning($"Invalid Image Url in WebhookMessage: {embed.Image?.Url}");
                    embed.Image.Url = null;
                }

                if (ValidationHelper.IsUrlInvalid(embed.Thumbnail?.Url ?? null))
                {
                    Logger.LogWarning($"Invalid Thumbnail Url in WebhookMessage: {embed.Thumbnail?.Url}");
                    embed.Thumbnail.Url = null;
                }
            }
        }

        public void FormatParameters(Dictionary<string, object> parameters)
        {
            string Format(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    text = text.Replace("{" + parameter.Key + "}", parameter.Value?.ToString() ?? string.Empty);
                }

                return text;
            }

            Username = Format(Username);
            Content = Format(Content);

            if (Embeds != null)
            {
                foreach (Embed embed in Embeds ?? [])
                {
                    embed.Title = Format(embed.Title);
                    embed.Description = Format(embed.Description);

                    if (embed.Footer != null)
                    {
                        embed.Footer.Text = Format(embed.Footer.Text);
                        embed.Footer.IconUrl = Format(embed.Footer.IconUrl);
                    }

                    if (embed.Image != null)
                    {
                        embed.Image.Url = Format(embed.Image.Url);
                    }

                    if (embed.Thumbnail != null)
                    {
                        embed.Thumbnail.Url = Format(embed.Thumbnail.Url);
                    }

                    if (embed.Author != null)
                    {
                        embed.Author.Name = Format(embed.Author.Name);
                        embed.Author.Url = Format(embed.Author.Url);
                        embed.Author.IconUrl = Format(embed.Author.IconUrl);
                    }

                    if (embed.Fields != null)
                    {
                        foreach (EmbedField field in embed.Fields)
                        {
                            field.Name = Format(field.Name);
                            field.Value = Format(field.Value);
                        }
                    }
                }
            }
        }

        public object Clone()
        {
            return new WebhookMessage
            {
                Enabled = Enabled,
                WebhookUrl = WebhookUrl,
                ContentLines = ContentLines?.Select(x => (string)x.Clone()).ToArray(),
                Files = Files?.Select(x => (byte[])x.Clone()).ToArray(),
                Username = Username,
                AvatarUrl = AvatarUrl,
                Content = Content,
                Embeds = Embeds?.Select(x => (Embed)x.Clone()).ToArray()
            };
        }
    }