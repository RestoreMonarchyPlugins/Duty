using RestoreMonarchy.Duty.Models.Discord;

namespace RestoreMonarchy.Duty.Models;

public class DiscordService
{
    public bool DiscordEnabled;
    public WebhookMessage DutyStarted;
    public WebhookMessage DutySummary;
    
    
    
    
    public DiscordService()
    {
        
    }
    // Below is not being used at all at the moment :) can be removed
    public DiscordService(bool discordEnabled, WebhookMessage dutyStarted, WebhookMessage dutySummary)
    {
        DiscordEnabled = discordEnabled;
        DutyStarted = dutyStarted;
        DutySummary = dutySummary;
    }
}