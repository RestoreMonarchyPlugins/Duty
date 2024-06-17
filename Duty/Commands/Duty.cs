using System.Collections.Generic;
using RestoreMonarchy.Duty.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace RestoreMonarchy.Duty.Commands;

public class Duty : IRocketCommand
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;
    public AllowedCaller AllowedCaller => AllowedCaller.Player;
    public string Name => "duty";
    public string Help => "Toggle your duty status";
    public string Syntax => "<duty name>";
    public List<string> Aliases => new List<string> { "d" };
    public List<string> Permissions => new List<string> { "duty.commands" };
    
    public void Execute(IRocketPlayer caller, string[] command)
    {
        UnturnedPlayer player = (UnturnedPlayer)caller;
        
        if (command.Length != 1)
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_command_usage"));
            return;
        }
        string dutyName = command[0];
        if (!config.DutyGroups.Exists(x => x.DutyGroupName == dutyName))
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_invalid_name"));
            return;
        }
        if (pluginInstance.ActiveDuties.Exists(x => x.PlayerId == player.CSteamID))
        {
            DutyHelper.OffDuty(player, dutyName);
            return;
        }
        if (!player.HasPermission(config.DutyGroups.Find(x => x.DutyGroupName == dutyName).Permission))
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_no_permission_group", dutyName));
            return;
        }

        DutyHelper.OnDuty(player, dutyName);
    }
}