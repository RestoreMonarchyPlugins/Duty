using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RestoreMonarchy.Duty.Models.Discord;

public class EmbedThumbnail : ICloneable
{
    [JsonProperty("url")]
    [XmlAttribute]
    public string Url { get; set; }
    public bool ShouldSerializeUrl() => !string.IsNullOrEmpty(Url);

    public object Clone()
    {
        return new EmbedThumbnail
        {
            Url = Url
        };
    }
}