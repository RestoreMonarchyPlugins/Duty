using System;
using System.Collections.Generic;
using RestoreMonarchy.Duty.Models;
using RestoreMonarchy.Duty.Services;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RestoreMonarchy.Duty.Helpers;

public static class DutyHelper
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;
    private static WebhookService webhookService => pluginInstance.WebhookService;
    public static void OnDuty(UnturnedPlayer player, string dutyName)
    {
        DutyGroups dutyGroups = config.DutyGroups.Find(x => x.DutyGroupName == dutyName);
        R.Permissions.AddPlayerToGroup(dutyGroups.PermGroup, player);
        pluginInstance.ActiveDuties.Add(new ActiveDuty(player.CSteamID, dutyName, System.DateTime.UtcNow, config.DutyGroups.Find(x => x.DutyGroupName == dutyName).DutySettings));
        UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_on", dutyName));
        DutyGroups dutyGroup = config.DutyGroups.Find(x => x.DutyGroupName == dutyName);
        DutySettings dutySettings = dutyGroup.DutySettings;
        if (config.UIService.UIEnabled)
        {
            UIHelper.EnableDutyUI(player);
        }
        if (dutySettings.GodMode)
        {
            player.Features.GodMode = true;
            if (config.UIService.UIEnabled)
            {
                UIHelper.EnableGodModeUI(player);
            }
            
        }
        if (dutySettings.Vanish)
        {
            player.Features.VanishMode = true;
            if (config.UIService.UIEnabled)
            {
                UIHelper.EnableVanishUI(player);
            }
        }
        if (dutySettings.AdminFreecam)
        {
            player.Player.look.sendFreecamAllowed(true);
        } 
        if (dutySettings.AdminEsp)
        {
            player.Player.look.sendSpecStatsAllowed(true);
        }

        if (dutySettings.AdminBuilding)
        {
            player.Player.look.sendWorkzoneAllowed(true);
        }
        if (config.Discord.Enabled && config.Discord.DutyStarted.Enabled)
        {
            Dictionary<string, object> param = new()
            {
                { "name", player.SteamName },
                { "steam_id", player.CSteamID.m_SteamID },
                { "dutyname", dutyName },
                { "charactername", player.CharacterName},
                { "thumbnail", player.SteamProfile.AvatarIcon.ToString()},
                { "server_name", Provider.serverName},
                { "permission", dutyGroup.Permission},
                { "date", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()},
                { "positionx", player.Position.x},
                { "positiony", player.Position.y},
                { "positionz", player.Position.z}
            };
            ThreadHelper.RunAsynchronously(() => 
            {
                webhookService.SendMessage(config.Discord.DutyStarted, param);
            });
           
        }
    }
    
    public static void OffDuty(UnturnedPlayer player, string dutyName)
    {
        ulong secondsOnDuty = 0;
        DutyGroups dutyGroups = config.DutyGroups.Find(dg => dg.DutyGroupName == dutyName);
        R.Permissions.RemovePlayerFromGroup(dutyGroups.PermGroup, player);
        string[] commandsUsed;
        string commandsExecuted;
        commandsUsed = [];
        if (pluginInstance.ActiveDutyCommands.ContainsKey(player.CSteamID))
        {
            commandsUsed = pluginInstance.ActiveDutyCommands[player.CSteamID].ToArray();
            Logger.Log($"Commands used by {player.CSteamID}: {string.Join(", ", commandsUsed)}");
            pluginInstance.ActiveDutyCommands[player.CSteamID].Clear();
        }
        commandsExecuted = "[" + string.Join("], [", commandsUsed) + "]";
        ActiveDuty activeDuty = pluginInstance.ActiveDuties.Find(x => x.PlayerId == player.CSteamID);
        if (activeDuty != null)
        {
            TimeSpan timeOnDuty = DateTime.UtcNow - activeDuty.TimeStarted;
            secondsOnDuty = (ulong)timeOnDuty.TotalSeconds;
        }
        pluginInstance.ActiveDutyCommands.Remove(player.CSteamID);
        pluginInstance.ActiveDuties.RemoveAll(x => x.PlayerId == player.CSteamID);
        if (config.UIService.UIEnabled)
        {
            UIHelper.DisableDutyUI(player);
        }
        if (dutyGroups.DutySettings.AdminEsp)
        {
            player.Player.look.sendSpecStatsAllowed(false);
        }
        if (dutyGroups.DutySettings.AdminFreecam)
        {
            player.Player.look.sendFreecamAllowed(false);
        }
        if (dutyGroups.DutySettings.GodMode)
        {
            player.Features.GodMode = false;
            if (config.UIService.UIEnabled)
            {
                UIHelper.DisableGodModeUI(player);
            }
        }
        if (dutyGroups.DutySettings.Vanish)
        {
            player.Features.VanishMode = false;
            if (config.UIService.UIEnabled)
            {
                UIHelper.DisableVanishUI(player);
            }
        }
        if (dutyGroups.DutySettings.AdminBuilding)
        {
            player.Player.look.sendWorkzoneAllowed(false);
        }
        if (config.Discord.Enabled && config.Discord.DutySummary.Enabled)
        {
            Dictionary<string, object> param = new()
            {
                { "name", player.SteamName },
                { "charactername", player.CharacterName},
                { "thumbnail", player.SteamProfile.AvatarIcon.ToString()},
                { "steam_id", player.CSteamID.m_SteamID },
                { "dutyname", dutyName },
                { "server_name", Provider.serverName},
                { "time", secondsOnDuty},
                { "commands_executed", commandsExecuted},
                {"timestarted", ((DateTimeOffset)activeDuty.TimeStarted).ToUnixTimeSeconds()},
                {"timeended", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()},
                { "positionx", player.Position.x},
                { "positiony", player.Position.y},
                { "positionz", player.Position.z}
            };
            ThreadHelper.RunAsynchronously(() => 
            {
                webhookService.SendMessage(config.Discord.DutySummary, param);
            });
           
        }
        
        UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_off"));
    }
}