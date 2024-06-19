using System.Collections.Generic;
using System.Xml.Serialization;
using RestoreMonarchy.Duty.Models;
using RestoreMonarchy.Duty.Models.Discord;
using Rocket.API;

namespace RestoreMonarchy.Duty
{
    public class DutyConfiguration : IRocketPluginConfiguration
    {
        public UIService UIService { get; set; }
        public DiscordConfig Discord { get; set; }
        public List<DutyGroups> DutyGroups { get; set; }

        public void LoadDefaults()
        {
            UIService = new UIService(true, 59501, 32000);
            Discord = new()
            {
                Enabled = true,
                DutyStarted = new()
                {
                    Enabled = true,
                    WebhookUrl = "YOUR_WEBHOOK_URL",
                    Embeds =
                    [
                        new()
                        {
                            Title = "Player came on duty",
                            ColorHex = "#00FF00",
                            Thumbnail = new()
                            {
                                Url = "{thumbnail}"
                            },
                            Fields =
                            [
                                new()
                                {
                                    Name = "**Player**",
                                    Value = "Steam Name: **[{name}](https://steamcommunity.com/profiles/{steam_id})** ({steam_id}) \n Character Name: **{charactername}** ",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "**Duty Info**",
                                    Value = " Time: <t:{date}:F> \n Duty Group: `{dutyname}`  \n Has Permission `{permission}`",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "**Player Position**",
                                    Value = "X: `{positionx}` \n Y: `{positiony}` \n Z: `{positionz}` \n",
                                    Inline = true
                                }
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
                    Enabled = true,
                    WebhookUrl = "YOUR_WEBHOOK_URL",
                    Embeds =
                    [
                        new()
                        {
                            Title = "Duty summary",
                            Thumbnail = new()
                            {
                                Url = "{thumbnail}"
                            },
                            Fields =
                            [
                                new()
                                {
                                    Name = "**Player**",
                                    Value = "Steam Name: **[{name}](https://steamcommunity.com/profiles/{steam_id})** ({steam_id}) \n Character Name: **{charactername}** ",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "**Duty Info**",
                                    Value = "Started at: <t:{timestarted}:F> \n Ended at: <t:{timeended}:F> \n Total Seconds: {time} ",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "**Player Position**",
                                    Value = "X: `{positionx}` \n Y: `{positiony}` \n Z: `{positionz}` \n",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "**Commands executed**",
                                    Value = "{commands_executed}",
                                    Inline = true
                                },
                                new()
                                {
                                    Name = "Admin Tools Usage",
                                    Value = "{adminUsages}",
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
                    ]
                }
            };
            DutyGroups = new List<DutyGroups>
            {
                new DutyGroups("Admin", "Admin", "duty.admin",
                    new DutySettings(true, true, true, true, true, true, true, true, true)),
            };
        }

    }
}