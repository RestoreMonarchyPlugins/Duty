namespace RestoreMonarchy.Duty.Models;

public class UIService
{
    public bool UIEnabled { get; set; }
    public ushort EffectID { get; set; }
    public short EffectKey { get; set; }
    
    public UIService() { }
    
    public UIService(bool uiEnabled, ushort effectId, short effectKey)
    {
        UIEnabled = uiEnabled;
        EffectID = effectId;
        EffectKey = effectKey;
    }
}