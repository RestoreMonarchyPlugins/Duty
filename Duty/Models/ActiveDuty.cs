using System;
using Steamworks;

namespace RestoreMonarchy.Duty.Models;

public class ActiveDuty
{
    public CSteamID PlayerId;
    public string DutyGroupName;
    public DateTime TimeStarted;
    public DutySettings DutySettings;
    
    public ActiveDuty()
    {
    }
    
    public ActiveDuty(CSteamID playerId, string dutyGroupName, DateTime timeStarted, DutySettings dutySettings)
    {
        PlayerId = playerId;
        DutyGroupName = dutyGroupName;
        TimeStarted = timeStarted;
        DutySettings = dutySettings;
    }
}