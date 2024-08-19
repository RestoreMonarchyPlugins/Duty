using System;
using System.Collections.Generic;
using RestoreMonarchy.Duty.Models;
using RestoreMonarchy.Duty.Services;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Steam;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.Duty.Helpers;

public static class DutyHelper
{
    private static DutyPlugin pluginInstance => DutyPlugin.Instance;
    private static DutyConfiguration config => pluginInstance.Configuration.Instance;
    private static WebhookService webhookService => pluginInstance.WebhookService;
    public static void OnDuty(UnturnedPlayer player, DutyGroup dutyGroup)
    {
        R.Permissions.AddPlayerToGroup(dutyGroup.PermissionGroup, player);
        
        ActiveDuty activeDuty = new(player.CSteamID, dutyGroup.Name, DateTime.UtcNow, dutyGroup.Settings);        
        pluginInstance.ActiveDuties.Add(activeDuty);
        
        UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_on", dutyGroup.Name));
        DutySettings dutySettings = dutyGroup.Settings;

        if (config.UI.Enabled)
        {
            UIHelper.EnableDutyUI(player);
        }
        if (dutySettings.GodMode)
        {
            player.Features.GodMode = true;
            if (config.UI.Enabled)
            {
                UIHelper.EnableGodModeUI(player);
            }
            
        }
        if (dutySettings.Vanish)
        {
            player.Features.VanishMode = true;
            if (config.UI.Enabled)
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
        if (dutySettings.AdminBlueHammer)
        {
            player.Player.channel.owner.isAdmin = true;
        }

        if (dutySettings.AdminBuilding)
        {
            player.Player.look.sendWorkzoneAllowed(true);
        }

        if (config.Discord.Enabled && config.Discord.DutyStarted.Enabled)
        {
            DateTime date = DateTime.UtcNow;
            Vector3 position = player.Position;
            ThreadHelper.RunAsynchronously(() => 
            {
                Profile profile = player.SteamProfile;
                int positionX = (int)position.x;
                int positionY = (int)position.y;
                int positionZ = (int)position.z;

                Dictionary<string, object> param = new()
                {
                    { "steam_name", player.SteamName },
                    { "steam_id", player.CSteamID.m_SteamID },
                    { "duty_name", dutyGroup.Name },
                    { "character_name", player.CharacterName },
                    { "avatar_url", profile.AvatarFull.ToString() },
                    { "avatar_url_small", profile.AvatarIcon.ToString() },
                    { "server_name", Provider.serverName },
                    { "permission", dutyGroup.Permission },
                    { "date", ((DateTimeOffset)date).ToUnixTimeSeconds() },
                    { "position_x", positionX },
                    { "position_y", positionY },
                    { "position_z", positionZ }
                };
                webhookService.SendMessage(config.Discord.DutyStarted, param);
            });           
        }
    }
    
    public static void OffDuty(UnturnedPlayer player, DutyGroup dutyGroup)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }
        if (dutyGroup == null)
        {
            throw new ArgumentNullException(nameof(dutyGroup));
        }

        ulong secondsOnDuty = 0;
        R.Permissions.RemovePlayerFromGroup(dutyGroup.PermissionGroup, player);
        
        string[] commandsUsed;
        string commandsExecuted;
        commandsUsed = [];

        if (pluginInstance.ActiveDutyCommands.ContainsKey(player.CSteamID))
        {
            commandsUsed = pluginInstance.ActiveDutyCommands[player.CSteamID].ToArray();
            Logger.Log($"Commands used by {player.DisplayName} ({player.CSteamID}): {string.Join(", ", commandsUsed)}");
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

        if (config.UI.Enabled)
        {
            UIHelper.DisableDutyUI(player);
        }

        if (dutyGroup.Settings.AdminEsp)
        {
            player.Player.look.sendSpecStatsAllowed(false);
        }

        if (dutyGroup.Settings.AdminFreecam)
        {
            player.Player.look.sendFreecamAllowed(false);
        }

        if (dutyGroup.Settings.AdminBlueHammer)
        {
            player.Player.channel.owner.isAdmin = false;
        }

        if (dutyGroup.Settings.GodMode)
        {
            player.Features.GodMode = false;
            if (config.UI.Enabled)
            {
                UIHelper.DisableGodModeUI(player);
            }
        }

        if (dutyGroup.Settings.Vanish)
        {
            player.Features.VanishMode = false;
            if (config.UI.Enabled)
            {
                UIHelper.DisableVanishUI(player);
            }
        }

        if (dutyGroup.Settings.AdminBuilding)
        {
            player.Player.look.sendWorkzoneAllowed(false);
        }

        if (config.Discord.Enabled && config.Discord.DutySummary.Enabled)
        {
            Vector3 position = player.Position;
            DateTime timeEnded = DateTime.UtcNow;
            ThreadHelper.RunAsynchronously(() => 
            {
                Profile profile = player.SteamProfile;
                int positionX = (int)position.x;
                int positionY = (int)position.y;
                int positionZ = (int)position.z;

                Dictionary<string, object> param = new()
                {
                    { "steam_name", player.SteamName },
                    { "character_name", player.CharacterName },
                    { "avatar_url",profile.AvatarFull.ToString()},
                    { "avatar_url_small", profile.AvatarIcon.ToString() },
                    { "steam_id", player.CSteamID.m_SteamID },
                    { "duty_name", dutyGroup.Name },
                    { "server_name", Provider.serverName },
                    { "time", secondsOnDuty },
                    { "commands_executed", commandsExecuted },
                    { "time_started", ((DateTimeOffset)activeDuty.TimeStarted).ToUnixTimeSeconds() },
                    { "time_ended", ((DateTimeOffset)timeEnded).ToUnixTimeSeconds() },
                    { "position_x", positionX },
                    { "position_y", positionY },
                    { "position_z", positionZ }
                };
                webhookService.SendMessage(config.Discord.DutySummary, param);
            });
           
        }
        
        UnturnedChat.Say(player.CSteamID, pluginInstance.Translate("duty_off"));
    }
}