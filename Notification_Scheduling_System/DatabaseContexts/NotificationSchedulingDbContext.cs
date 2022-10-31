using Microsoft.EntityFrameworkCore;
using Notification_Scheduling_System.Entities;

namespace Notification_Scheduling_System.DatabaseContexts;

public class NotificationSchedulingDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Company> Companies { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationSchedule> NotificationSchedules { get; set; }
    public DbSet<CompanyType?> CompanyType { get; set; }
    public DbSet<Market?> Markets { get; set; }
    public DbSet<NotificationScheduleConfiguration?> NotificationConfigurations { get; set; }
    public DbSet<ScheduleConfigurationDay> ScheduledDays { get; set; }

    public NotificationSchedulingDbContext()
    {
    }

    public NotificationSchedulingDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration.GetConnectionString("SQLite"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}