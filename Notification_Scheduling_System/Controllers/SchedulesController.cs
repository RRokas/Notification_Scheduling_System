using Microsoft.AspNetCore.Mvc;
using Notification_Scheduling_System.DatabaseContexts;
using Notification_Scheduling_System.DTOs;
using Notification_Scheduling_System.Services;

namespace Notification_Scheduling_System.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationScheduleController : ControllerBase
{
    private readonly ILogger<NotificationScheduleController> _logger;
    private readonly NotificationSchedulingDbContext _dbContext;
    private readonly ISchedulingService _schedulingService;

    public NotificationScheduleController(ILogger<NotificationScheduleController> logger,
        NotificationSchedulingDbContext notificationSchedulingDbContext, ISchedulingService schedulingService)
    {
        _logger = logger;
        _dbContext = notificationSchedulingDbContext;
        _schedulingService = schedulingService;
    }

    [HttpGet(Name = "GetScheduleForCompany")]
    public NotificationScheduleDto Get(Guid companyId)
    {
        return _schedulingService.GetNotificationScheduleDto(companyId);
    }

    [HttpPost("AddScheduleConfiguration")]
    public void AddScheduleConfiguration(AddScheduleConfigurationDto notificationScheduleConfiguration)
    {
        _schedulingService.AddNotificationScheduleConfiguration(notificationScheduleConfiguration);
    }

    [HttpPost("CreateCompany")]
    public Guid CreateCompany(NewCompanyDto companyData)
    {
        var companyGuid = _schedulingService.CreateCompanyFromDto(companyData);

        return companyGuid;
    }
}