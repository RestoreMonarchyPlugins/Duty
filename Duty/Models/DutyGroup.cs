using System.Xml.Serialization;

namespace RestoreMonarchy.Duty.Models;

public class DutyGroup
{
    [XmlAttribute]
    public string Name { get; set; }
    public string PermissionGroup { get; set; }
    public string Permission { get; set; }
    public DutySettings Settings { get; set; }
}