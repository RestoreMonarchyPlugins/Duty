using System.Xml.Serialization;

namespace RestoreMonarchy.Duty.Models;

public class UIConfig
{
    [XmlAttribute]
    public bool Enabled { get; set; }
    [XmlAttribute]
    public ushort EffectID { get; set; }
    
    public UIConfig() { }
    
    public UIConfig(bool uiEnabled, ushort effectId)
    {
        Enabled = uiEnabled;
        EffectID = effectId;
    }
}