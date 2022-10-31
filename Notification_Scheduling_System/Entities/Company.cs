using System.ComponentModel.DataAnnotations;
using static Notification_Scheduling_System.Validation.ValidationRegex;

namespace Notification_Scheduling_System.Entities;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Market? Market { get; set; }

    [RegularExpression(TenDigitsOnlyStringPattern, ErrorMessage = "Company number must be exactly 10 digits!")]
    public string CompanyNumber { get; set; }

    public CompanyType? Type { get; set; }
    public List<NotificationSchedule> NotificationSchedules { get; set; }

    public Company()
    {
        Id = new Guid();
        NotificationSchedules = new List<NotificationSchedule>();
    }
}