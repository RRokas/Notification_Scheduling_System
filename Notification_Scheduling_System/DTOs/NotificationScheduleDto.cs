using System.Text.Json.Serialization;

namespace Notification_Scheduling_System.DTOs;

public class NotificationScheduleDto
{
    public Guid CompanyId { get; set; }
    [JsonPropertyName("notifications")]
    public IEnumerable<string> FormattedDates { get; set; }
}