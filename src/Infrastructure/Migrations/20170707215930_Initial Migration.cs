using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DistrictState = table.Column<string>(maxLength: 10, nullable: true),
                    ERPCustomerId = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Municipality = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<byte>(type: "tinyint", nullable: false),
                    CultureInfo = table.Column<string>(maxLength: 50, nullable: true),
                    LanguageName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "LanguageResource",
                columns: table => new
                {
                    ResourceValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogicName = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageResource", x => x.ResourceValueId);
                });

            migrationBuilder.CreateTable(
                name: "LocationFields",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<byte>(type: "tinyint", nullable: false),
                    DeviceUsageId = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(maxLength: 50, nullable: true),
                    CollectedAt = table.Column<byte>(type: "tinyint", nullable: true),
                    DataType = table.Column<string>(maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(maxLength: 50, nullable: true),
                    FieldType = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TestState = table.Column<byte>(type: "tinyint", nullable: true),
                    Validation = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationFields", x => new { x.AccountId, x.FieldId, x.DeviceUsageId });
                    table.UniqueConstraint("AK_LocationFields_AccountId_DeviceUsageId_FieldId", x => new { x.AccountId, x.DeviceUsageId, x.FieldId });
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsRandom = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PlanName = table.Column<string>(maxLength: 50, nullable: false),
                    PreventRepeat = table.Column<bool>(type: "bit", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "PlanHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterUnitTokens",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: true),
                    CreatorUserName = table.Column<string>(maxLength: 50, nullable: true),
                    SiteId = table.Column<Guid>(nullable: true),
                    Token = table.Column<string>(maxLength: 50, nullable: false),
                    UnitId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterUnitTokens", x => x.TokenId);
                });

            migrationBuilder.CreateTable(
                name: "ReportSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailList = table.Column<string>(maxLength: 1024, nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastSent = table.Column<DateTime>(type: "datetime", nullable: true),
                    RecurrenceRule = table.Column<string>(maxLength: 128, nullable: true),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportPeriod = table.Column<byte>(type: "tinyint", nullable: false),
                    ScheduleTitle = table.Column<string>(maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSchedules", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "ReportScheduleSites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportScheduleSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActualIncubationTime = table.Column<int>(type: "int", nullable: true),
                    ControlExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ControlLotNumber = table.Column<string>(maxLength: 25, nullable: true),
                    ControlModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ControlModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ControlName = table.Column<string>(maxLength: 25, nullable: true),
                    CorrectedTest = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorrectiveAction = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomField1 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField10 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField2 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField3 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField4 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField5 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField6 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField7 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField8 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField9 = table.Column<string>(maxLength: 4000, nullable: true),
                    DeviceCategory = table.Column<string>(maxLength: 10, nullable: true),
                    DeviceName = table.Column<string>(maxLength: 50, nullable: true),
                    DeviceTemperature = table.Column<int>(type: "int", nullable: true),
                    DeviceUOM = table.Column<string>(maxLength: 10, nullable: true),
                    Dilution = table.Column<int>(type: "int", nullable: true),
                    GroupName = table.Column<string>(maxLength: 250, nullable: true),
                    IncubationTime = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    LocationName = table.Column<string>(maxLength: 50, nullable: false),
                    Lower = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(maxLength: 250, nullable: true),
                    Personnel = table.Column<string>(maxLength: 256, nullable: true),
                    PlanName = table.Column<string>(maxLength: 50, nullable: false),
                    RLU = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    RawADCOutput = table.Column<int>(type: "int", nullable: false),
                    RepeatedTest = table.Column<byte>(type: "tinyint", nullable: true),
                    ResultDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: true),
                    SampleType = table.Column<string>(maxLength: 25, nullable: true),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfaceName = table.Column<string>(maxLength: 250, nullable: true),
                    TestState = table.Column<byte>(type: "tinyint", nullable: true),
                    UnitAngle = table.Column<int>(type: "int", nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: false),
                    UnitNo = table.Column<int>(type: "int", nullable: false),
                    UnitSoftware = table.Column<string>(maxLength: 10, nullable: true),
                    UnitType = table.Column<string>(maxLength: 100, nullable: true),
                    Upper = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(maxLength: 30, nullable: true),
                    WarningId = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "SiteUsers",
                columns: table => new
                {
                    SiteUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUsers", x => x.SiteUserId);
                });

            migrationBuilder.CreateTable(
                name: "SureTrendVersionInfo",
                columns: table => new
                {
                    AppVersion = table.Column<string>(maxLength: 100, nullable: false),
                    DatabaseVersion = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SureTrendVersionInfo", x => x.AppVersion);
                });

            migrationBuilder.CreateTable(
                name: "Surfaces",
                columns: table => new
                {
                    SurfaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(maxLength: 4000, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    SurfaceName = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surfaces", x => x.SurfaceId);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(maxLength: 250, nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    SiteId = table.Column<Guid>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 50, nullable: false),
                    UnitNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserAccountId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserAccountId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    RoleDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    SiteId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.SiteId);
                    table.ForeignKey(
                        name: "FK_Sites_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageUserValues",
                columns: table => new
                {
                    LanguageUserId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: true),
                    Caption = table.Column<string>(maxLength: 500, nullable: true),
                    LanguageId = table.Column<byte>(nullable: true),
                    LanguageResourceResourceValueId = table.Column<int>(nullable: true),
                    ToolTip = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageUserValues", x => x.LanguageUserId);
                    table.ForeignKey(
                        name: "FK_LanguageUserValues_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageUserValues_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageUserValues_LanguageResource_LanguageResourceResourceValueId",
                        column: x => x.LanguageResourceResourceValueId,
                        principalTable: "LanguageResource",
                        principalColumn: "ResourceValueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LanguageValues",
                columns: table => new
                {
                    LanguageResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(maxLength: 500, nullable: true),
                    LanguageId = table.Column<byte>(nullable: true),
                    LanguageResourceResourceValueId = table.Column<int>(nullable: true),
                    ToolTip = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageValues", x => x.LanguageResourceId);
                    table.ForeignKey(
                        name: "FK_LanguageValues_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguageValues_LanguageResource_LanguageResourceResourceValueId",
                        column: x => x.LanguageResourceResourceValueId,
                        principalTable: "LanguageResource",
                        principalColumn: "ResourceValueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanLocation_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    UnitNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanUnit_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanHistoryResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanHistoryResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanHistoryResult_PlanHistory_PlanHistoryId",
                        column: x => x.PlanHistoryId,
                        principalTable: "PlanHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControlId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorrectiveAction = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomField1 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField10 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField2 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField3 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField4 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField5 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField6 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField7 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField8 = table.Column<string>(maxLength: 4000, nullable: true),
                    CustomField9 = table.Column<string>(maxLength: 4000, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    DeviceUsageId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IncubationTime = table.Column<int>(type: "int", nullable: true),
                    LocationName = table.Column<string>(maxLength: 50, nullable: true),
                    Lower = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(maxLength: 250, nullable: true),
                    Personnel = table.Column<string>(maxLength: 256, nullable: true),
                    QualitativeQuantitative = table.Column<byte>(type: "tinyint", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    SurfaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Upper = table.Column<int>(type: "int", nullable: true),
                    ZoneID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Surfaces_SurfaceId",
                        column: x => x.SurfaceId,
                        principalTable: "Surfaces",
                        principalColumn: "SurfaceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    UserRoleRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_UserRoles_UserRoleRoleId",
                        column: x => x.UserRoleRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePermissions",
                columns: table => new
                {
                    RoleUserPermissionId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<string>(nullable: true),
                    RoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermissions", x => x.RoleUserPermissionId);
                    table.ForeignKey(
                        name: "FK_UserRolePermissions_AspNetRoles_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePermissions_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanLocationMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanLocationMap", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_PlanLocationMap_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitLocationMap",
                columns: table => new
                {
                    UnitNo = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitLocationMap", x => new { x.UnitNo, x.LocationId });
                    table.UniqueConstraint("AK_UnitLocationMap_LocationId_UnitNo", x => new { x.LocationId, x.UnitNo });
                    table.ForeignKey(
                        name: "FK_UnitLocationMap_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserSettingId = table.Column<Guid>(nullable: false),
                    DashDateFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    DashDateTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    DashPeriod = table.Column<byte>(type: "tinyint", nullable: true),
                    DashType = table.Column<byte>(type: "tinyint", nullable: true),
                    LanguageId = table.Column<byte>(nullable: true),
                    ReportScheduleCurrentView = table.Column<string>(maxLength: 25, nullable: true),
                    ResultGridSchema = table.Column<string>(nullable: true),
                    ResultsDateFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResultsDateTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResultsPeriod = table.Column<byte>(type: "tinyint", nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserSettingId);
                    table.ForeignKey(
                        name: "FK_UserSettings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageResource_LogicName",
                table: "LanguageResource",
                column: "LogicName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUserValues_AccountId",
                table: "LanguageUserValues",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUserValues_LanguageId",
                table: "LanguageUserValues",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUserValues_LanguageResourceResourceValueId",
                table: "LanguageUserValues",
                column: "LanguageResourceResourceValueId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageValues_LanguageId",
                table: "LanguageValues",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageValues_LanguageResourceResourceValueId",
                table: "LanguageValues",
                column: "LanguageResourceResourceValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SurfaceId",
                table: "Locations",
                column: "SurfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanHistoryResult_PlanHistoryId",
                table: "PlanHistoryResult",
                column: "PlanHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanLocation_PlanId",
                table: "PlanLocation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanLocationMap_LocationId",
                table: "PlanLocationMap",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUnit_PlanId",
                table: "PlanUnit",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_AccountId",
                table: "Sites",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_PermissionId",
                table: "UserRolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_RoleId",
                table: "UserRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_LanguageId",
                table: "UserSettings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserRoleRoleId",
                table: "AspNetUsers",
                column: "UserRoleRoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageUserValues");

            migrationBuilder.DropTable(
                name: "LanguageValues");

            migrationBuilder.DropTable(
                name: "LocationFields");

            migrationBuilder.DropTable(
                name: "PlanHistoryResult");

            migrationBuilder.DropTable(
                name: "PlanLocation");

            migrationBuilder.DropTable(
                name: "PlanLocationMap");

            migrationBuilder.DropTable(
                name: "PlanUnit");

            migrationBuilder.DropTable(
                name: "RegisterUnitTokens");

            migrationBuilder.DropTable(
                name: "ReportSchedules");

            migrationBuilder.DropTable(
                name: "ReportScheduleSites");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "SiteUsers");

            migrationBuilder.DropTable(
                name: "SureTrendVersionInfo");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "UnitLocationMap");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserRolePermissions");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LanguageResource");

            migrationBuilder.DropTable(
                name: "PlanHistory");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Surfaces");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
