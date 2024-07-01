using Rocket.Unturned.Player;
using SDG.NetTransport;
using SDG.Unturned;

namespace RestoreMonarchy.Duty.Helpers;

public class UIHelper
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;

    private static ushort EffectId => config.UI.EffectID;
    private const short EffectKey = 29751;

    public static void EnableDutyUI(UnturnedPlayer player)
    {
        ITransportConnection transportConnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffect(EffectId, EffectKey, transportConnection, true);
        EffectManager.sendUIEffectVisibility(EffectKey, transportConnection, true, "Duty_Duty", true);
        DisableVanishUI(player);
        DisableGodModeUI(player);
    }
    
    public static void EnableVanishUI(UnturnedPlayer player)
    {
        ITransportConnection transportConnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectText(EffectKey, transportConnection, true, "Duty_Vanish_Text", "Vanish <color=green>ON");
    }
    
    public static void EnableGodModeUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectText(EffectKey, transportconnection, true, "Duty_God_Text", "God <color=green>ON");
    }
    
    public static void DisableDutyUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.askEffectClearByID(EffectId, transportconnection);
    }
    
    public static void DisableVanishUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectText(EffectKey, transportconnection, true, "Duty_Vanish_Text", "Vanish <color=red>OFF");
    }
    
    public static void DisableGodModeUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectText(EffectKey, transportconnection, true, "Duty_God_Text", "God <color=red>OFF");

    } 
}