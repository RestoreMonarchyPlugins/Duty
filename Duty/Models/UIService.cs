namespace RestoreMonarchy.Duty.Models;

public class UIService
{
    public bool UIEnabled;
    public ushort EffectID;
    public short EffectKey;
    
    public UIService()
    {
        
    }
    
    public UIService(bool uiEnabled, ushort effectId, short effectKey)
    {
        UIEnabled = uiEnabled;
        EffectID = effectId;
        EffectKey = effectKey;
    }
}