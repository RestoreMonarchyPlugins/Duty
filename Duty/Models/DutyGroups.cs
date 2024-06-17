namespace RestoreMonarchy.Duty.Models;

public class DutyGroups
{
    public string DutyGroupName;
    public string PermGroup;
    public string Permission;
    public DutySettings DutySettings;
    
    public DutyGroups()
    {
    }
    public DutyGroups(string dutyGroupName, string permGroup, string permission, DutySettings dutySettings)
    {
        DutyGroupName = dutyGroupName;
        PermGroup = permGroup;
        Permission = permission;
        DutySettings = dutySettings;
    }
}