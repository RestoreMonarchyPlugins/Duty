using System;
using Steamworks;

namespace RestoreMonarchy.Duty.Models;

public class ActiveDuty
{
    public CSteamID PlayerId { get; set; }
    public string DutyGroupName { get; set; }
    public DateTime TimeStarted { get; set; }
    public DutySettings DutySettings { get; set; }
    
    public ActiveDuty() { }
    
    public ActiveDuty(CSteamID playerId, string dutyGroupName, DateTime timeStarted, DutySettings dutySettings)
    {
        PlayerId = playerId;
        DutyGroupName = dutyGroupName;
        TimeStarted = timeStarted;
        DutySettings = dutySettings;
    }
}