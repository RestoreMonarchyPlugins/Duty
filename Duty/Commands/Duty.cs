using System;
using System.Collections.Generic;
using System.Linq;
using RestoreMonarchy.Duty.Helpers;
using RestoreMonarchy.Duty.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace RestoreMonarchy.Duty.Commands;

public class Duty : IRocketCommand
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;

    public void Execute(IRocketPlayer caller, string[] command)
    {
        UnturnedPlayer player = (UnturnedPlayer)caller;

        if (command.Length != 1)
        {
            IEnumerable<DutyGroup> dutyGroups = config.DutyGroups.Where(x => player.IsAdmin || player.HasPermission(x.Permission));
            if (!dutyGroups.Any())
            {
                UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_no_permissions"));
                return;
            }

            string dutyList = string.Join(", ", dutyGroups.Select(x => x.Name));

            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_list", dutyList));
            return;
        }

        DutyGroup dutyGroup;
        string dutyName = command[0];
        ActiveDuty activeDuty = pluginInstance.ActiveDuties.FirstOrDefault(x => x.PlayerId == player.CSteamID);

        if (dutyName.Equals("off", StringComparison.OrdinalIgnoreCase))
        {
            if (activeDuty != null)
            {
                dutyGroup = config.DutyGroups.Find(x => x.Name == activeDuty.DutyGroupName);
                DutyHelper.OffDuty(player, dutyGroup);
                return;
            }
            else
            {
                UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_not_on"));
                return;
            }
        }

        dutyGroup = config.DutyGroups.FirstOrDefault(x => x.Name.Equals(dutyName, StringComparison.OrdinalIgnoreCase));
        if (dutyGroup == null)
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_invalid_name", dutyName));
            return;
        }

        if (!player.HasPermission(dutyGroup.Permission))
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_no_permission_group", dutyName));
            return;
        }

        if (activeDuty != null)
        {
            UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_already", activeDuty.DutyGroupName));
            return;
        }

        DutyHelper.OnDuty(player, dutyGroup);
    }


    public AllowedCaller AllowedCaller => AllowedCaller.Player;
    public string Name => "duty";
    public string Help => "Toggle your duty status";
    public string Syntax => "<duty name>";
    public List<string> Aliases => new List<string> { "d" };
    public List<string> Permissions => new List<string> { "duty.commands" };
}