using System.ComponentModel.DataAnnotations;

namespace Notification_Scheduling_System.Entities;

public class CompanyType
{
    [Key]
    public string? Name { get; set; }
    public List<Company> Companies { get; set; }
}