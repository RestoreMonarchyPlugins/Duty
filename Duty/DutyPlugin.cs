﻿using System.Collections.Generic;
using System.Linq;
using RestoreMonarchy.Duty.Helpers;
using RestoreMonarchy.Duty.Models;
using RestoreMonarchy.Duty.Services;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.Duty;

public class DutyPlugin : RocketPlugin<DutyConfiguration>
{
    public static DutyPlugin Instance;
    private static DutyConfiguration config => Instance.Configuration.Instance;
    public List<ActiveDuty> ActiveDuties;
    public Dictionary<CSteamID, List<string>> ActiveDutyCommands;
    public WebhookService WebhookService { get; set; }

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
        if (config.DiscordService.DiscordEnabled)
        {
            WebhookService = new();
            ActiveDutyCommands = [];
            R.Commands.OnExecuteCommand += OnCommandExecuted;
        }

        
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
        if (config.DiscordService.DiscordEnabled)
        {
            R.Commands.OnExecuteCommand -= OnCommandExecuted;
            ActiveDutyCommands.Clear();
            WebhookService = null;
        }
        ActiveDuties.Clear();
        Instance = null;
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
            DutyGroups dutyGroup = config.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

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
            DutyGroups dutyGroup = config.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

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
            DutyGroups dutyGroup = config.DutyGroups.Find(x => x.DutyGroupName == activeDuty.DutyGroupName);

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
            DutyGroups dutyGroup = config.DutyGroups.Find(dg => dg.DutyGroupName == activeDuty.DutyGroupName);

            if (dutyGroup != null && dutyGroup.DutySettings.BlockDamageToPlayers)
            {
                shouldallow = false;
            }
        }
    }

    private void OnCommandExecuted(IRocketPlayer player, IRocketCommand command, ref bool cancel)
    {
        if (ActiveDuties.Any() && config.DiscordService.DiscordEnabled && player is UnturnedPlayer unturnedPlayer)
        {
            if (ActiveDuties.Exists(x => x.PlayerId == unturnedPlayer.CSteamID))
            {
               
                    ActiveDuty activeDuty = ActiveDuties.FirstOrDefault(x => x.PlayerId == unturnedPlayer.CSteamID);
                    if (activeDuty == null) return;
                    if (ActiveDutyCommands.ContainsKey(unturnedPlayer.CSteamID))
                    {
                        ActiveDutyCommands[unturnedPlayer.CSteamID].Add(command.Name);
                        Logger.LogError("Added command" + " " + command.Name + " " + "to the list");
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
            DutyGroups dutyGroup = config.DutyGroups.Find(ad => ad.DutyGroupName == activeDuty.DutyGroupName);
            // Couldn't use x here due to the fact that function has a byte x parameter
            if (dutyGroup != null && dutyGroup.DutySettings.ItemPickup)
            {
                shouldallow = false;
            }
        }
    }
    
    #endregion
}