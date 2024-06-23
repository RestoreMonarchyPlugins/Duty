using RestoreMonarchy.Duty.Helpers;
using RestoreMonarchy.Duty.Models;
using RestoreMonarchy.Duty.Services;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Core.Steam;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.Duty;

public class DutyPlugin : RocketPlugin<DutyConfiguration>
{
    public static DutyPlugin Instance { get; private set; }
    private static DutyConfiguration configuration => Instance.Configuration.Instance;
    public List<ActiveDuty> ActiveDuties { get; private set; }
    public Dictionary<CSteamID, List<string>> ActiveDutyCommands { get; private set; }
    public WebhookService WebhookService { get; private set; }

    protected override void Load()
    {
        Instance = this;
        ActiveDuties = [];
        
        U.Events.OnPlayerDisconnected += PlayerLeft;
        DamageTool.damagePlayerRequested += DamagePlayerRequested;
        StructureManager.onDamageStructureRequested += StructureDamageRequested;
        BarricadeManager.onDamageBarricadeRequested += BarricadeDamageRequested;
        BarricadeManager.onOpenStorageRequested += BarricadeOpenRequest;
        ItemManager.onTakeItemRequested += ItemTakeRequested;
        if (configuration.Discord.Enabled)
        {
            WebhookService = new();
            ActiveDutyCommands = [];
            R.Commands.OnExecuteCommand += OnCommandExecuted;
        }

        Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
    }

    protected override void Unload()
    {
        foreach (ActiveDuty activeDuty in ActiveDuties)
        {
            DutyHelper.OffDuty(UnturnedPlayer.FromCSteamID(activeDuty.PlayerId), activeDuty.DutyGroupName);
        }

        U.Events.OnPlayerDisconnected -= PlayerLeft;
        DamageTool.damagePlayerRequested -= DamagePlayerRequested;
        StructureManager.onDamageStructureRequested -= StructureDamageRequested;
        BarricadeManager.onDamageBarricadeRequested -= BarricadeDamageRequested;
        BarricadeManager.onOpenStorageRequested -= BarricadeOpenRequest;
        ItemManager.onTakeItemRequested -= ItemTakeRequested;

        if (configuration.Discord.Enabled)
        {
            R.Commands.OnExecuteCommand -= OnCommandExecuted;
            ActiveDutyCommands.Clear();
            WebhookService = null;
        }
        ActiveDuties.Clear();
        Instance = null;

        Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
    }

    public override TranslationList DefaultTranslations => new()
    {
        {"duty_command_usage", "Invalid syntax! Correct syntax: /duty <duty name>"},
        {"duty_invalid_name", "Invalid duty name!"},
        {"duty_no_permission", "You do not have permission to go on duty as {0}"},
        {"duty_on", "You have gone on duty as {0}"},
        {"duty_off", "You have gone off duty"},
        {"duty_no_group", "Duty group {0} does not exist!"},
        {"duty_no_permission_group", "You do not have permission to go on duty as {0}"},
   };


    #region EventHandlers
    
    private void BarricadeOpenRequest(CSteamID instigator, InteractableStorage storage, ref bool shouldallow)
    {
        Player player = PlayerTool.getPlayer(instigator);
        if (player == null) return;
        if (ActiveDuties.Any() && ActiveDuties.Exists(x => x.PlayerId == player.channel.owner.playerID.steamID))
        {
            ActiveDuty activeDuty = ActiveDuties.First(x => x.PlayerId == player.channel.owner.playerID.steamID);
            DutyGroups dutyGroup = configuration.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

            if (dutyGroup != null && dutyGroup.DutySettings.BlockStorageInteraction)
            {
                shouldallow = false;
            }
        }
    }
    
    private void StructureDamageRequested(CSteamID instigator, Transform structuretransform, ref ushort pendingtotaldamage, ref bool shouldallow, EDamageOrigin damageorigin)
    {
        Player player = PlayerTool.getPlayer(instigator);
        if (player == null) return;
        if (ActiveDuties.Any() && ActiveDuties.Exists(x => x.PlayerId == player.channel.owner.playerID.steamID))
        {
            ActiveDuty activeDuty = ActiveDuties.First(x => x.PlayerId == player.channel.owner.playerID.steamID);
            DutyGroups dutyGroup = configuration.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

            if (dutyGroup != null && dutyGroup.DutySettings.BlockStructureDamage)
            {
                shouldallow = false;
            }
        }
    }
    
    private void BarricadeDamageRequested(CSteamID instigatorsteamid, Transform barricadetransform, ref ushort pendingtotaldamage, ref bool shouldallow, EDamageOrigin damageorigin)
    {
        Player player = PlayerTool.getPlayer(instigatorsteamid);
        if (player == null) return;
        if (ActiveDuties.Any() && ActiveDuties.Exists(x => x.PlayerId == player.channel.owner.playerID.steamID))
        {
            ActiveDuty activeDuty = ActiveDuties.First(x => x.PlayerId == player.channel.owner.playerID.steamID);
            DutyGroups dutyGroup = configuration.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

            if (dutyGroup != null && dutyGroup.DutySettings.BlockBarricadeDamage)
            {
                shouldallow = false;
            }
        }
    }

    private void PlayerLeft(UnturnedPlayer player)
    {
        if (ActiveDuties.Any() && ActiveDuties.Exists(x => x.PlayerId == player.CSteamID))
        {
            DutyHelper.OffDuty(player, ActiveDuties.Find(x => x.PlayerId == player.CSteamID).DutyGroupName);
            ActiveDuties.RemoveAll(x => x.PlayerId == player.CSteamID);
        }
    }

    private void DamagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldallow)
    {
        Player player = PlayerTool.getPlayer(parameters.killer);
        if (player == null) return;
        if (ActiveDuties.Any() && ActiveDuties.Exists(x => x.PlayerId == player.channel.owner.playerID.steamID))
        {
            ActiveDuty activeDuty = ActiveDuties.First(x => x.PlayerId == player.channel.owner.playerID.steamID);
            DutyGroups dutyGroup = configuration.DutyGroups.Find(dg => dg.DutyGroupName == activeDuty.DutyGroupName);

            if (dutyGroup != null && dutyGroup.DutySettings.BlockDamageToPlayers)
            {
                shouldallow = false;
            }
        }
    }

    private void OnCommandExecuted(IRocketPlayer player, IRocketCommand command, ref bool cancel)
    {
        if (cancel)
        {
            return;
        }

        if (ActiveDuties.Any() && configuration.Discord.Enabled && player is UnturnedPlayer unturnedPlayer)
        {
            if (ActiveDuties.Exists(x => x.PlayerId == unturnedPlayer.CSteamID))
            {
                if (command.Name == "god")
                {
                    if (unturnedPlayer.GodMode == true)
                    {
                        UIHelper.DisableGodModeUI(unturnedPlayer);
                    }
                    else
                    {
                        UIHelper.EnableGodModeUI(unturnedPlayer);
                    }
                }
                if (command.Name == "vanish")
                {
                    if (unturnedPlayer.Features.VanishMode == true)
                    {
                        UIHelper.DisableVanishUI(unturnedPlayer);
                    }
                    else
                    {
                        UIHelper.EnableVanishUI(unturnedPlayer);
                    }
                }
                if (configuration.Discord.DutyCommandLog.Enabled)
                {
                    
                    ThreadHelper.RunAsynchronously(() => 
                    {
                        Profile profile = unturnedPlayer.SteamProfile;
                        Dictionary<string, object> param = new()
                        {
                            { "steam_name", unturnedPlayer.SteamName },
                            { "steam_id", unturnedPlayer.CSteamID.m_SteamID },
                            { "character_name", unturnedPlayer.CharacterName},
                            { "avatar_url", profile.AvatarFull.ToString()},
                            { "avatar_url_small", profile.AvatarIcon.ToString()},
                            { "server_name", Provider.serverName},
                            { "command", command.Name },
                        };

                        WebhookService.SendMessage(configuration.Discord.DutyCommandLog, param);
                    });
                }
               
                ActiveDuty activeDuty = ActiveDuties.FirstOrDefault(x => x.PlayerId == unturnedPlayer.CSteamID);
                if (activeDuty == null)
                {
                    return;
                }

                if (ActiveDutyCommands.ContainsKey(unturnedPlayer.CSteamID))
                {
                    ActiveDutyCommands[unturnedPlayer.CSteamID].Add(command.Name);
                }
                else
                {
                    ActiveDutyCommands.Add(unturnedPlayer.CSteamID, new List<string> { command.Name });
                }
            }
        }
    }
    
    private void ItemTakeRequested(Player player, byte x, byte y, uint instanceid, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemdata, ref bool shouldallow)
    {
        if (ActiveDuties.Any() && ActiveDuties.Exists(ad => ad.PlayerId == player.channel.owner.playerID.steamID))
        {
            ActiveDuty activeDuty = ActiveDuties.First(ad => ad.PlayerId == player.channel.owner.playerID.steamID);
            DutyGroups dutyGroup = configuration.DutyGroups.Find(ad => ad.DutyGroupName == activeDuty.DutyGroupName);
            // Couldn't use x here due to the fact that function has a byte x parameter
            if (dutyGroup != null && dutyGroup.DutySettings.BlockItemPickup)
            {
                shouldallow = false;
            }
        }
    }
    
    #endregion
}