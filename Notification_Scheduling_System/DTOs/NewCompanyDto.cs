using System.ComponentModel.DataAnnotations;
using Notification_Scheduling_System.Entities;
using static Notification_Scheduling_System.Validation.ValidationRegex;

namespace Notification_Scheduling_System.DTOs;

public class NewCompanyDto
{
    public string Name { get; set; }
    public string? Marketname { get; set; }

    [RegularExpression(TenDigitsOnlyStringPattern, ErrorMessage = "Company number must be exactly 10 digits!")]
    public string Number { get; set; }

    public string? CompanyType { get; set; }
}