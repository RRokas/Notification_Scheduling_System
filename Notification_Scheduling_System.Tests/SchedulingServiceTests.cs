using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Notification_Scheduling_System.DatabaseContexts;
using Notification_Scheduling_System.Entities;
using Notification_Scheduling_System.Services;
using Xunit;

namespace Notification_Scheduling_System.Tests;

public class SchedulingServiceTests
{
    private readonly Mock<NotificationSchedulingDbContext> _mockDbContext;
    
    public SchedulingServiceTests()
    {
        var mockDbContext = new Mock<NotificationSchedulingDbContext>();
        _mockDbContext = mockDbContext;
    }
    
    [Fact]
    public void GenerateNotifications_creates_expected_notifications_basic_scenario()
    {
        // Arrange
        var service = new SchedulingService(_mockDbContext.Object);
        var testDate = new DateOnly(2022, 10, 1);
        var days = new List<ScheduleConfigurationDay>
        {
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 5,
            },
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 15,
            },
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 20,
            }
        };
        var config = new NotificationScheduleConfiguration(){DaysOfMonthForNotifications = days};

        // Act
        var notifications = service.GenerateNotifications(config, testDate);
        var notificationDates = notifications.Select(notification => notification.SendDate).ToList();

        // Assert
        Assert.Equal(5, notificationDates[0].Day);
        Assert.Equal(10, notificationDates[0].Month);
        Assert.Equal(2022, notificationDates[0].Year);
        
        Assert.Equal(15, notificationDates[1].Day);
        Assert.Equal(10, notificationDates[1].Month);
        Assert.Equal(2022, notificationDates[1].Year);
        
        Assert.Equal(20, notificationDates[2].Day);
        Assert.Equal(10, notificationDates[2].Month);
        Assert.Equal(2022, notificationDates[2].Year);
    }

    [Fact]
    public void GenerateNotifications_createsExpectedNotifications_notificationsSpanAcrossDifferentMonths()
    {
        // Arrange
        var service = new SchedulingService(_mockDbContext.Object);
        var testDate = new DateOnly(2022, 10, 6);
        var days = new List<ScheduleConfigurationDay>
        {
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 2,
            },
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 5,
            },
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 15,
            },
            new ScheduleConfigurationDay
            {
                DayOfTheMonth = 20,
            }
        };
        var config = new NotificationScheduleConfiguration(){DaysOfMonthForNotifications = days};
        

        // Act
        var notifications = service.GenerateNotifications(config, testDate);
        var notificationDates = notifications.Select(notification => notification.SendDate).ToList();

        // Assert
        Assert.Equal(2, notificationDates[0].Day);
        Assert.Equal(11, notificationDates[0].Month);
        Assert.Equal(2022, notificationDates[0].Year);
        
        Assert.Equal(5, notificationDates[1].Day);
        Assert.Equal(11, notificationDates[1].Month);
        Assert.Equal(2022, notificationDates[1].Year);
        
        Assert.Equal(15, notificationDates[2].Day);
        Assert.Equal(10, notificationDates[2].Month);
        Assert.Equal(2022, notificationDates[2].Year);
        
        Assert.Equal(20, notificationDates[3].Day);
        Assert.Equal(10, notificationDates[3].Month);
        Assert.Equal(2022, notificationDates[3].Year);
    }

    [Fact]
    public void NotificationScheduleToScheduleDto_returns_correct_format_dates()
    {
        // Arrange
        var notificationList = new List<Notification>
        {
            new Notification()
            {
                SendDate = new DateOnly(2022, 10, 20)
            },
            new Notification()
            {
                SendDate = new DateOnly(2024, 12, 25)
            }
        };
        var schedule = new NotificationSchedule()
        {
            ScheduledNotifications = notificationList
        };
        
        var service = new SchedulingService(_mockDbContext.Object);

        // Act
        var scheduleDto = service.NotificationScheduleToScheduleDto(schedule, new Guid());
        var formattedDates = scheduleDto.FormattedDates.ToList();
        
        //Assert
        Assert.Equal("20/10/2022", formattedDates[0]);
        Assert.Equal("25/12/2024", formattedDates[1]);
    }
}