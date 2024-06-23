namespace RestoreMonarchy.Duty.Models;

public class DutyGroups
{
    public string DutyGroupName { get; set; }
    public string PermGroup { get; set; }
    public string Permission { get; set; }
    public DutySettings DutySettings { get; set; }
}