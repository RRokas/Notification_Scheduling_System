using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification_Scheduling_System.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyType",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyType", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.CountryName);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledDays",
                columns: table => new
                {
                    DayOfTheMonth = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDays", x => x.DayOfTheMonth);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MarketCountryName = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyNumber = table.Column<string>(type: "TEXT", nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_CompanyType_TypeName",
                        column: x => x.TypeName,
                        principalTable: "CompanyType",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_Companies_Markets_MarketCountryName",
                        column: x => x.MarketCountryName,
                        principalTable: "Markets",
                        principalColumn: "CountryName");
                });

            migrationBuilder.CreateTable(
                name: "NotificationConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MarketCountryName = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyTypeName = table.Column<string>(type: "TEXT", nullable: true),
                    CreationTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationConfigurations_CompanyType_CompanyTypeName",
                        column: x => x.CompanyTypeName,
                        principalTable: "CompanyType",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_NotificationConfigurations_Markets_MarketCountryName",
                        column: x => x.MarketCountryName,
                        principalTable: "Markets",
                        principalColumn: "CountryName");
                });

            migrationBuilder.CreateTable(
                name: "NotificationScheduleConfigurationScheduleConfigurationDay",
                columns: table => new
                {
                    AssignedConfigurationsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DaysOfMonthForNotificationsDayOfTheMonth = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationScheduleConfigurationScheduleConfigurationDay", x => new { x.AssignedConfigurationsId, x.DaysOfMonthForNotificationsDayOfTheMonth });
                    table.ForeignKey(
                        name: "FK_NotificationScheduleConfigurationScheduleConfigurationDay_NotificationConfigurations_AssignedConfigurationsId",
                        column: x => x.AssignedConfigurationsId,
                        principalTable: "NotificationConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationScheduleConfigurationScheduleConfigurationDay_ScheduledDays_DaysOfMonthForNotificationsDayOfTheMonth",
                        column: x => x.DaysOfMonthForNotificationsDayOfTheMonth,
                        principalTable: "ScheduledDays",
                        principalColumn: "DayOfTheMonth",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigurationUsedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationTimestampUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSchedules_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationSchedules_NotificationConfigurations_ConfigurationUsedId",
                        column: x => x.ConfigurationUsedId,
                        principalTable: "NotificationConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SendDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    NotificationScheduleId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationSchedules_NotificationScheduleId",
                        column: x => x.NotificationScheduleId,
                        principalTable: "NotificationSchedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_MarketCountryName",
                table: "Companies",
                column: "MarketCountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TypeName",
                table: "Companies",
                column: "TypeName");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfigurations_CompanyTypeName",
                table: "NotificationConfigurations",
                column: "CompanyTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfigurations_MarketCountryName",
                table: "NotificationConfigurations",
                column: "MarketCountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationScheduleId",
                table: "Notifications",
                column: "NotificationScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationScheduleConfigurationScheduleConfigurationDay_DaysOfMonthForNotificationsDayOfTheMonth",
                table: "NotificationScheduleConfigurationScheduleConfigurationDay",
                column: "DaysOfMonthForNotificationsDayOfTheMonth");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSchedules_CompanyId",
                table: "NotificationSchedules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSchedules_ConfigurationUsedId",
                table: "NotificationSchedules",
                column: "ConfigurationUsedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationScheduleConfigurationScheduleConfigurationDay");

            migrationBuilder.DropTable(
                name: "NotificationSchedules");

            migrationBuilder.DropTable(
                name: "ScheduledDays");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "NotificationConfigurations");

            migrationBuilder.DropTable(
                name: "CompanyType");

            migrationBuilder.DropTable(
                name: "Markets");
        }
    }
}
