namespace RestoreMonarchy.Duty.Models;

public class DutySettings
{
    public bool GodMode { get; set; }
    public bool Vanish { get; set; }
    public bool AdminFreecam { get; set; }
    public bool AdminEsp { get; set; }
    public bool BlockDamageToPlayers { get; set; }
    public bool BlockStructureDamage { get; set; }
    public bool BlockBarricadeDamage { get; set; }
    public bool BlockStorageInteraction { get; set; }
    public bool ItemPickup { get; set; }
    
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