using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RestoreMonarchy.Duty.Models.Discord;

public class EmbedAuthor : ICloneable
{
    [JsonProperty("name")]
    [XmlAttribute]
    public string Name { get; set; }
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    [JsonProperty("url")]
    [XmlAttribute]
    public string Url { get; set; }
    public bool ShouldSerializeUrl() => !string.IsNullOrEmpty(Url);
    [JsonProperty("icon_url")]
    [XmlAttribute]
    public string IconUrl { get; set; }
    public bool ShouldSerializeIconUrl() => !string.IsNullOrEmpty(IconUrl);

    public object Clone()
    {
        return new EmbedAuthor
        {
            Name = Name,
            Url = Url,
            IconUrl = IconUrl
        };
    }
}