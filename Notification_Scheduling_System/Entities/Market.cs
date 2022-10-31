using System.ComponentModel.DataAnnotations;

namespace Notification_Scheduling_System.Entities;

public class Market
{
    [Key]
    public string? CountryName { get; set; }
    public List<Company> Companies { get; set; }
}