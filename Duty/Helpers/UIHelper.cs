using Rocket.Unturned.Player;
using SDG.NetTransport;
using SDG.Unturned;

namespace RestoreMonarchy.Duty.Helpers;

public class UIHelper
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;
    public static void EnableDutyUI(UnturnedPlayer player)
    {
        ITransportConnection transportConnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffect(config.UIService.EffectID, config.UIService.EffectKey, transportConnection, true);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportConnection, true, "Duty_Duty", true);
        DisableVanishUI(player);
        DisableGodModeUI(player);
    }
    
    public static void EnableVanishUI(UnturnedPlayer player)
    {
        ITransportConnection transportConnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportConnection, true, "Duty_Vanish", true);
    }
    
    public static void EnableGodModeUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportconnection, true, "Duty_God", true);
    }
    
    public static void DisableDutyUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportconnection, true, "Duty_Duty", false);
    }
    
    public static void DisableVanishUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportconnection, true, "Duty_Vanish", false);
    }
    
    public static void DisableGodModeUI(UnturnedPlayer player)
    {
        ITransportConnection transportconnection = Provider.findTransportConnection(player.CSteamID);
        EffectManager.sendUIEffectVisibility(config.UIService.EffectKey, transportconnection, true, "Duty_God", false);
    } 
}