using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RestoreMonarchy.Duty.Models.Discord;

public class EmbedField : ICloneable
{
    [JsonProperty("name")]
    [XmlAttribute]
    public string Name { get; set; }
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    [JsonProperty("value")]
    [XmlAttribute]
    public string Value { get; set; }
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    [JsonProperty("inline")]
    [XmlAttribute]
    public bool Inline { get; set; }
    public bool ShouldSerializeInline() => Inline;

    public object Clone()
    {
        return new EmbedField
        {
            Name = Name,
            Value = Value,
            Inline = Inline
        };
    }
}