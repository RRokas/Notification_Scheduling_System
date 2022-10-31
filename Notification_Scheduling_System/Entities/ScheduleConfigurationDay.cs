using System.ComponentModel.DataAnnotations;

namespace Notification_Scheduling_System.Entities;

public class ScheduleConfigurationDay
{
    [Key]
    [Range(1, 31)] public int DayOfTheMonth { get; set; }
    public List<NotificationScheduleConfiguration> AssignedConfigurations { get; set; }
}