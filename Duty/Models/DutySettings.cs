namespace RestoreMonarchy.Duty.Models;

public class DutySettings
{
    public bool GodMode;
    public bool Vanish;
    public bool AdminFreecam;
    public bool AdminEsp;
    public bool BlockDamageToPlayers;
    public bool BlockStructureDamage;
    public bool BlockBarricadeDamage;
    public bool BlockStorageInteraction;
    public bool ItemPickup;
    
    public DutySettings()
    {
    }
    public DutySettings(bool godMode, bool vanish, bool freecam, bool adminEsp, bool blockDamageToPlayers, bool blockStructureDamage, bool blockBarricadeDamage, bool blockObjectInteraction, bool itemPickup)
    {
        GodMode = godMode;
        Vanish = vanish;
        AdminFreecam = freecam;
        AdminEsp = adminEsp;
        BlockDamageToPlayers = blockDamageToPlayers;
        BlockStructureDamage = blockStructureDamage;
        BlockBarricadeDamage = blockBarricadeDamage;
        BlockStorageInteraction = blockObjectInteraction;
        ItemPickup = itemPickup;
    }
}