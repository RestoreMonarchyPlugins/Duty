namespace RestoreMonarchy.Duty.Models;

public class UIService
{
    public bool UIEnabled { get; set; }
    public ushort EffectID { get; set; }
    
    public UIService() { }
    
    public UIService(bool uiEnabled, ushort effectId)
    {
        UIEnabled = uiEnabled;
        EffectID = effectId;
    }
}