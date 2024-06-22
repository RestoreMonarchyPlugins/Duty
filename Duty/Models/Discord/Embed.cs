using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RestoreMonarchy.Duty.Models.Discord;

   public class Embed : ICloneable
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);
        [JsonProperty("url")]
        public string Url { get; set; }
        public bool ShouldSerializeUrl() => !string.IsNullOrEmpty(Url);
        [JsonProperty("description")]
        public string Description { get; set; }
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        [JsonProperty("color")]
        public int? Color { get; set; }
        public bool ShouldSerializeColor() => Color.HasValue;

        [JsonProperty("author")]
        public EmbedAuthor Author { get; set; }
        public bool ShouldSerializeAuthor() => Author != null;
        [JsonProperty("thumbnail")]
        public EmbedThumbnail Thumbnail { get; set; }
        public bool ShouldSerializeThumbnail() => Thumbnail != null;
        [JsonProperty("image")]
        public EmbedImage Image { get; set; }
        public bool ShouldSerializeImage() => Image != null;
        [JsonProperty("fields")]
        [XmlArrayItem("Field")]
        public EmbedField[] Fields { get; set; }
        public bool ShouldSerializeFields() => Fields != null && Fields.Length > 0;
        [JsonProperty("footer")]
        public EmbedFooter Footer { get; set; }
        public bool ShouldSerializeFooter() => Footer != null;
        [JsonProperty("timestamp")]
        [XmlIgnore]
        public string Timestamp { get; set; }
        

        [JsonIgnore]
        public bool? WithCurrentTimestamp { get; set; }
        public bool ShouldSerializeWithCurrentTimestamp() => WithCurrentTimestamp.HasValue;
        [JsonIgnore]
        [XmlArrayItem("DescriptionLine")]
        public string[] DescriptionLines { get; set; }
        [JsonIgnore]
        public string ColorHex { get; set; }

        public void Prepare()
        {
            if (WithCurrentTimestamp.HasValue && WithCurrentTimestamp.Value)
            {
                SetTimestamp(DateTime.UtcNow);
            }
            if (DescriptionLines != null && DescriptionLines.Length > 0)
            {
                StringBuilder sb = new();
                foreach (string line in DescriptionLines)
                {
                    sb.AppendLine(line);
                }
                Description = sb.ToString();
            }
            if (ColorHex != null)
            {
                Color = int.Parse(ColorHex.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
            }
        }

        public void SetTimestamp(DateTime dateTime)
        {
            Timestamp = dateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
        }

        public object Clone()
        {
            return new Embed
            {
                Title = Title,
                Url = Url,
                Description = Description,
                Color = Color,
                ColorHex = ColorHex,
                DescriptionLines = DescriptionLines?.Clone() as string[],
                WithCurrentTimestamp = WithCurrentTimestamp,
                Author = Author?.Clone() as EmbedAuthor,
                Thumbnail = Thumbnail?.Clone() as EmbedThumbnail,
                Image = Image?.Clone() as EmbedImage,
                Fields = Fields?.Select(x => (EmbedField)x.Clone()).ToArray(),
                Footer = Footer?.Clone() as EmbedFooter,
                Timestamp = Timestamp
            };
        }
    }