using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Notification_Scheduling_System.Entities;

public class NotificationScheduleConfiguration
{
    [Key]
    public Guid Id { get; set; }
    public Market? Market { get; set; }
    public CompanyType? CompanyType { get; set; }
    public List<ScheduleConfigurationDay> DaysOfMonthForNotifications { get; set; }
    public DateTime CreationTimeStamp { get; set; }
    public DateTime ActiveFrom { get; set; }

    public NotificationScheduleConfiguration()
    {
        Id = new Guid();
    }
}