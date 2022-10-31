using Microsoft.EntityFrameworkCore;
using Notification_Scheduling_System.DatabaseContexts;
using Notification_Scheduling_System.DTOs;
using Notification_Scheduling_System.Entities;

namespace Notification_Scheduling_System.Services;

public interface ISchedulingService
{
    public Guid CreateCompanyFromDto(NewCompanyDto newCompanyDto);
    public NotificationScheduleDto GetNotificationScheduleDto(Guid requestedCompanyId);
    public void AddNotificationScheduleConfiguration(AddScheduleConfigurationDto configurationDto);

    public List<Notification> GenerateNotifications(NotificationScheduleConfiguration configuration,
        DateOnly fromDate);

    public NotificationScheduleDto NotificationScheduleToScheduleDto(NotificationSchedule schedule, Guid companyId);
}

public class SchedulingService : ISchedulingService
{
    private readonly NotificationSchedulingDbContext _dbContext;

    public SchedulingService(NotificationSchedulingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public NotificationScheduleDto GetNotificationScheduleDto(Guid requestedCompanyId)
    {
        var requestedCompany = _dbContext.Companies
            .Include(company => company.Market)
            .Include(company => company.Type)
            .First(company => company.Id == requestedCompanyId);

        var activeScheduleConfiguration = GetMostDetailedConfiguration(requestedCompany);
        if (activeScheduleConfiguration == null)
            return new NotificationScheduleDto()
                { CompanyId = requestedCompanyId, FormattedDates = Enumerable.Empty<string>() };

        var generatedSchedule = CreateNotificationSchedule(activeScheduleConfiguration, requestedCompany);
        var scheduleDto = NotificationScheduleToScheduleDto(generatedSchedule, requestedCompanyId);

        return scheduleDto;
    }

    public NotificationScheduleDto NotificationScheduleToScheduleDto(NotificationSchedule schedule, Guid companyId)
    {
        var notificationDates = schedule.ScheduledNotifications
            .Select(notification => notification.SendDate.ToString("dd/MM/yyyy")).ToList();

        var scheduleDto = new NotificationScheduleDto
        {
            CompanyId = companyId,
            FormattedDates = notificationDates
        };

        return scheduleDto;
    }

    public List<Notification> GenerateNotifications(NotificationScheduleConfiguration configuration, DateOnly fromDate)
    {
        var countFromDate = fromDate;
        var notifications = new List<Notification>();

        var notificationCount = configuration.DaysOfMonthForNotifications.Count;
        for (var i = 0; i < notificationCount; i++)
        {
            var generateForDayOfMonth = configuration.DaysOfMonthForNotifications[i];
            var sendDate = new DateOnly(countFromDate.Year, countFromDate.Month, generateForDayOfMonth.DayOfTheMonth);

            if (countFromDate.Day > generateForDayOfMonth.DayOfTheMonth)
                sendDate = new DateOnly(countFromDate.Year, countFromDate.Month + 1, generateForDayOfMonth.DayOfTheMonth);

            var notifcation = new Notification
            {
                SendDate = sendDate
            };

            notifications.Add(notifcation);
        }

        return notifications;
    }

    private NotificationSchedule CreateNotificationSchedule(NotificationScheduleConfiguration? configuration,
        Company scheduleForCompany)
    {
        var schedule = new NotificationSchedule
        {
            ConfigurationUsed = configuration,
            ScheduledNotifications = GenerateNotifications(configuration, DateOnly.FromDateTime(DateTime.UtcNow))
        };

        scheduleForCompany.NotificationSchedules.Add(schedule);
        _dbContext.SaveChanges();

        return schedule;
    }

    private NotificationScheduleConfiguration? GetMostDetailedConfiguration(Company company)
    {
        var market = company.Market;
        var type = company.Type;


        var allDetailConfig =
            _dbContext.NotificationConfigurations.Include(config => config.DaysOfMonthForNotifications).Where(config =>
                config.Market.Equals(market) && config.CompanyType.Equals(type)).ToList();
        if (allDetailConfig.Any())
            return allDetailConfig
                .OrderByDescending(config => config.ActiveFrom)
                .First();

        var marketOnlyConfig = _dbContext.NotificationConfigurations
            .Include(config => config.DaysOfMonthForNotifications)
            .Where(config => config.Market.CountryName.Equals(market.CountryName) && config.CompanyType == null)
            .ToList();
        if (marketOnlyConfig.Any())
            return marketOnlyConfig
                .OrderByDescending(config => config.ActiveFrom)
                .First();

        var noDetailConfig = _dbContext.NotificationConfigurations
            .Include(config => config.DaysOfMonthForNotifications)
            .Where(config => config.Market == null && config.CompanyType == null)
            .ToList();
        if (noDetailConfig.Any())
            return noDetailConfig
                .OrderByDescending(config => config.ActiveFrom)
                .First();

        return null;
    }

    public Guid CreateCompanyFromDto(NewCompanyDto newCompanyDto)
    {
        var newCompany = new Company
        {
            Name = newCompanyDto.Name,
            Market = GetMarketForAssigning(newCompanyDto.Marketname),
            CompanyNumber = newCompanyDto.Number,
            Type = GetCompanyTypeForAssigning(newCompanyDto.CompanyType)
        };

        _dbContext.Companies.Add(newCompany);
        _dbContext.SaveChanges();

        return newCompany.Id;
    }

    public void AddNotificationScheduleConfiguration(AddScheduleConfigurationDto configurationDto)
    {
        var sendOnDays = new List<ScheduleConfigurationDay>();
        foreach (var day in configurationDto.SendOnDays)
        {
            var configDay = GetScheduleConfigurationDay(day);
            sendOnDays.Add(configDay);
        }

        var configurationToAdd = new NotificationScheduleConfiguration
        {
            Market = GetMarketForAssigning(configurationDto.MarketCountryName),
            CompanyType = GetCompanyTypeForAssigning(configurationDto.CompanyTypeName),
            DaysOfMonthForNotifications = sendOnDays,
            CreationTimeStamp = DateTime.UtcNow,
            ActiveFrom = configurationDto.ActiveFrom
        };

        _dbContext.NotificationConfigurations.Add(configurationToAdd);
        _dbContext.SaveChanges();
    }

    private ScheduleConfigurationDay GetScheduleConfigurationDay(int dayOfTheMonth)
    {
        var dayExists = _dbContext.ScheduledDays.Any(configDay => configDay.DayOfTheMonth.Equals(dayOfTheMonth));

        if (dayExists)
            return _dbContext.ScheduledDays.First(day => day.DayOfTheMonth.Equals(dayOfTheMonth));

        return CreateScheduleConfigurationDay(dayOfTheMonth);
    }

    private ScheduleConfigurationDay CreateScheduleConfigurationDay(int dayOfTheMonth)
    {
        var dayToadd = new ScheduleConfigurationDay()
        {
            DayOfTheMonth = dayOfTheMonth
        };

        _dbContext.ScheduledDays.Add(dayToadd);
        _dbContext.SaveChanges();

        return dayToadd;
    }

    public Market? GetMarketForAssigning(string? countryName)
    {
        if (countryName == null)
            return null;

        var marketExists = _dbContext.Markets.Any(market => market.CountryName.Equals(countryName));

        if (marketExists)
            return _dbContext.Markets.First(market => market.CountryName.Equals(countryName));

        return CreateMarket(countryName);
    }

    private Market CreateMarket(string? countryName)
    {
        var marketToAdd = new Market()
        {
            CountryName = countryName
        };

        _dbContext.Markets.Add(marketToAdd);
        _dbContext.SaveChanges();

        return marketToAdd;
    }

    private CompanyType? GetCompanyTypeForAssigning(string? companyTypeName)
    {
        if (companyTypeName == null)
            return null;

        var companyTypeExists = _dbContext.CompanyType.Any(companyType => companyType.Name.Equals(companyTypeName));

        if (companyTypeExists)
            return _dbContext.CompanyType.First(companyType => companyType!.Name.Equals(companyTypeName));

        return CreateCompanyType(companyTypeName);
    }

    private CompanyType CreateCompanyType(string? companyTypeName)
    {
        var companyTypeToAdd = new CompanyType()
        {
            Name = companyTypeName
        };

        _dbContext.CompanyType.Add(companyTypeToAdd);
        _dbContext.SaveChanges();

        return companyTypeToAdd;
    }
}