namespace Notification_Scheduling_System.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public DateOnly SendDate { get; set; }

    public Notification()
    {
        Id = new Guid();
    }
}