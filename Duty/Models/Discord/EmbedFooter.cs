using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RestoreMonarchy.Duty.Models.Discord;

public class EmbedFooter : ICloneable
{
    [JsonProperty("text")]
    [XmlAttribute]
    public string Text { get; set; }
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    [JsonProperty("icon_url")]
    [XmlAttribute]
    public string IconUrl { get; set; }
    public bool ShouldSerializeIconUrl() => !string.IsNullOrEmpty(IconUrl);

    public object Clone()
    {
        return new EmbedFooter
        {
            Text = Text,
            IconUrl = IconUrl
        };
    }
}