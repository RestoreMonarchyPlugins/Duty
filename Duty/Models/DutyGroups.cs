namespace RestoreMonarchy.Duty.Models;

public class DutyGroups
{
    public string DutyGroupName { get; set; }
    public string PermGroup { get; set; }
    public string Permission { get; set; }
    public DutySettings DutySettings { get; set; }
    
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