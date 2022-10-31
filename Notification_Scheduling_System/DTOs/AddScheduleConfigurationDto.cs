using Notification_Scheduling_System.Entities;

namespace Notification_Scheduling_System.DTOs;

public class AddScheduleConfigurationDto
{
    public string? MarketCountryName { get; set; }
    public string? CompanyTypeName { get; set; }
    public DateTime ActiveFrom { get; set; }
    public List<int> SendOnDays { get; set; }
}