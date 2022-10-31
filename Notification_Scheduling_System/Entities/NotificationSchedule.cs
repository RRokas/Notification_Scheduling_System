namespace Notification_Scheduling_System.Entities;

public class NotificationSchedule
{
    public Guid Id { get; set; }
    public NotificationScheduleConfiguration? ConfigurationUsed { get; set; }
    public List<Notification> ScheduledNotifications { get; set; }
    public DateTime CreationTimestampUTC { get; set; }

    public NotificationSchedule()
    {
        Id = new Guid();
        CreationTimestampUTC = DateTime.UtcNow;
    }
}