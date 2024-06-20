using RestoreMonarchy.Duty.Models.Discord;
using System.Xml.Serialization;

namespace RestoreMonarchy.Duty.Models;

public class DiscordConfig
{
    [XmlAttribute]
    public bool Enabled { get; set; }
    public WebhookMessage DutyStarted { get; set; }
    public WebhookMessage DutySummary { get; set; }
    public WebhookMessage DutyCommands { get; set; }
    
    public DiscordConfig()
    {
        
    }
}