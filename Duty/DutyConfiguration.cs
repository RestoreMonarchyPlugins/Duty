using RestoreMonarchy.Duty.Models;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.Duty
{
    public class DutyConfiguration : IRocketPluginConfiguration
    {
        public UIConfig UI { get; set; }
        public List<DutyGroup> DutyGroups { get; set; }
        public DiscordConfig Discord { get; set; }

        public void LoadDefaults()
        {
            UI = new UIConfig(true, 59501);
            DutyGroups =
            [
                new()
                {
                    Name = "Admin",
                    PermissionGroup = "Admin",
                    Permission = "duty.admin",
                    Settings = new()
                    {
                        GodMode = true,
                        Vanish = true,
                        AdminFreecam = true,
                        AdminEsp = true,
                        AdminBuilding = true,
                        BlockDamageToPlayers = false,
                        BlockStructureDamage = false,
                        BlockBarricadeDamage = false,
                        BlockStorageInteraction = false,
                        BlockItemPickup = false
                    }
                },
                new()
                {
                    Name = "Moderator",
                    PermissionGroup = "Moderator",
                    Permission = "duty.moderator",
                    Settings = new()
                    {
                        GodMode = true,
                        Vanish = true,
                        AdminFreecam = true,
                        AdminEsp = true,
                        AdminBuilding = true,
                        BlockDamageToPlayers = true,
                        BlockStructureDamage = true,
                        BlockBarricadeDamage = true,
                        BlockStorageInteraction = true,
                        BlockItemPickup = true
                    }
                }
            ];
            Discord = new()
            {
                Enabled = true,
                DutyStarted = new()
                {
                    Enabled = false,
                    WebhookUrl = "YOUR_WEBHOOK_URL",
                    Embeds =
                    [
                        new()
                        {
                            Title = "{character_name} started {duty_name} duty",
                            ColorHex = "#00FF00",
                            Thumbnail = new()
                            {
                                Url = "{avatar_url}"
                            },
                            Fields =
                            [
                                new()
                                {
                                    Name = "Steam ID",
                                    Value = "`{steam_id}`",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Steam Name",
                                    Value = "[{steam_name}](https://steamcommunity.com/profiles/{steam_id})",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Date",
                                    Value = "<t:{date}>",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Group",
                                    Value = "{duty_name}",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Permission",
                                    Value = "`{permission}`",
                                    Inline = true
                                },                                
                                new()
                                {
                                    Name = "Position",
                                    Value = "`{position_x} {position_z} {position_y}`",
                                    Inline = true
                                },
                            ],
                            Footer = new()
                            {
                                Text = "{server_name}",
                            },
                            WithCurrentTimestamp = true
                        }
                    ]
                },
                DutySummary = new()
                {
                    Enabled = false,
                    WebhookUrl = "YOUR_WEBHOOK_URL",
                    Embeds =
                    [
                        new()
                        {
                            Title = "{character_name} {duty_name} duty summary",
                            Description = "Commands Executed: ```{commands_executed}```",
                            Thumbnail = new()
                            {
                                Url = "{avatar_url}"
                            },
                            Fields =
                            [
                                new()
                                {
                                    Name = "Steam ID",
                                    Value = "`{steam_id}`",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Steam Name",
                                    Value = "[{steam_name}](https://steamcommunity.com/profiles/{steam_id})",
                                    Inline = true
                                },
                                new() 
                                {
                                    Name = "Duration",
                                    Value = "{time} seconds",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Started At",
                                    Value = "<t:{time_started}>",
                                    Inline = true
                                },
                                new() 
                                {
                                    Name = "Ended At",
                                    Value = "<t:{time_ended}>",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Position",
                                    Value = "`{position_x} {position_z} {position_y}`",
                                    Inline = true
                                },
                            ],
                            ColorHex = "#ff0000",
                            Footer = new()
                            {
                                Text = "{server_name}",
                            },
                            WithCurrentTimestamp = true
                        }
                    ]
                },
                DutyCommandLog = new()
                {
                    Enabled = false,
                    WebhookUrl = "YOUR_WEBHOOK_URL",
                    Embeds =
                    [
                        new()
                        {
                            Title = "{character_name} executed command on duty",
                            Thumbnail = new()
                            {
                                Url = "{avatar_url}"
                            },
                            Fields =
                            [
                                new()
                                {
                                    Name = "Steam ID",
                                    Value = "`{steam_id}`",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Steam Name",
                                    Value = "[{steam_name}](https://steamcommunity.com/profiles/{steam_id})",
                                    Inline = true
                                },
                                new() 
                                {
                                    Name = "Command",
                                    Value = "`{command}`",
                                    Inline = true
                                }
                            ],
                            ColorHex = "#ff0000",
                            Footer = new()
                            {
                                Text = "{server_name}",
                            },
                            WithCurrentTimestamp = true
                        }
                    ]}
            };
        }

    }
}