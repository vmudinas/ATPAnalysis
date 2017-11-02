using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure.DataContext;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170714054021_Unit Plan")]
    partial class UnitPlan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Infrastructure.Entities.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DistrictState")
                        .HasMaxLength(10);

                    b.Property<string>("ERPCustomerId")
                        .HasMaxLength(20);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Municipality")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Phone")
                        .HasMaxLength(50);

                    b.Property<string>("PostalCode")
                        .HasMaxLength(20);

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Infrastructure.Entities.Language", b =>
                {
                    b.Property<byte>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    b.Property<string>("CultureInfo")
                        .HasMaxLength(50);

                    b.Property<string>("LanguageName")
                        .HasMaxLength(200);

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Infrastructure.Entities.LanguageResource", b =>
                {
                    b.Property<int>("ResourceValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("LogicName")
                        .HasMaxLength(1000);

                    b.HasKey("ResourceValueId");

                    b.HasIndex("LogicName")
                        .IsUnique();

                    b.ToTable("LanguageResource");
                });

            modelBuilder.Entity("Infrastructure.Entities.LanguageUserValue", b =>
                {
                    b.Property<Guid>("LanguageUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountId");

                    b.Property<string>("Caption")
                        .HasMaxLength(500);

                    b.Property<byte?>("LanguageId");

                    b.Property<int?>("LanguageResourceResourceValueId");

                    b.Property<string>("ToolTip")
                        .HasMaxLength(1000);

                    b.HasKey("LanguageUserId");

                    b.HasIndex("AccountId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("LanguageResourceResourceValueId");

                    b.ToTable("LanguageUserValues");
                });

            modelBuilder.Entity("Infrastructure.Entities.LanguageValue", b =>
                {
                    b.Property<int>("LanguageResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Caption")
                        .HasMaxLength(500);

                    b.Property<byte?>("LanguageId");

                    b.Property<int?>("LanguageResourceResourceValueId");

                    b.Property<string>("ToolTip")
                        .HasMaxLength(1000);

                    b.HasKey("LanguageResourceId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("LanguageResourceResourceValueId");

                    b.ToTable("LanguageValues");
                });

            modelBuilder.Entity("Infrastructure.Entities.Location", b =>
                {
                    b.Property<Guid>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ControlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CorrectiveAction")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CustomField1")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField10")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField2")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField3")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField4")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField5")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField6")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField7")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField8")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField9")
                        .HasMaxLength(4000);

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<int>("DeviceUsageId")
                        .HasColumnType("int");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("IncubationTime")
                        .HasColumnType("int");

                    b.Property<string>("LocationName")
                        .HasMaxLength(50);

                    b.Property<int?>("Lower")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Notes")
                        .HasMaxLength(250);

                    b.Property<string>("Personnel")
                        .HasMaxLength(256);

                    b.Property<byte?>("QualitativeQuantitative")
                        .HasColumnType("tinyint");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int?>("RoomNumber")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid?>("SurfaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Upper")
                        .HasColumnType("int");

                    b.Property<Guid?>("ZoneID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LocationId");

                    b.HasIndex("SurfaceId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Infrastructure.Entities.LocationField", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("FieldId")
                        .HasColumnType("tinyint");

                    b.Property<int>("DeviceUsageId")
                        .HasColumnType("int");

                    b.Property<string>("Caption")
                        .HasMaxLength(50);

                    b.Property<byte?>("CollectedAt")
                        .HasColumnType("tinyint");

                    b.Property<string>("DataType")
                        .HasMaxLength(50);

                    b.Property<string>("FieldName")
                        .HasMaxLength(50);

                    b.Property<byte>("FieldType")
                        .HasColumnType("tinyint");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<byte?>("TestState")
                        .HasColumnType("tinyint");

                    b.Property<string>("Validation")
                        .HasMaxLength(4000);

                    b.HasKey("AccountId", "FieldId", "DeviceUsageId");

                    b.HasAlternateKey("AccountId", "DeviceUsageId", "FieldId");

                    b.ToTable("LocationFields");
                });

            modelBuilder.Entity("Infrastructure.Entities.Plan", b =>
                {
                    b.Property<Guid>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsRandom")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PlanName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("PreventRepeat")
                        .HasColumnType("bit");

                    b.Property<int>("Quota")
                        .HasColumnType("int");

                    b.HasKey("PlanId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quota")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("PlanHistory");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanHistoryResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ResultId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlanHistoryId");

                    b.ToTable("PlanHistoryResult");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanLocation");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanLocationMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MapId");

                    b.HasIndex("LocationId");

                    b.ToTable("PlanLocationMap");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quota")
                        .HasColumnType("int");

                    b.Property<int>("UnitNo");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanUnit");
                });

            modelBuilder.Entity("Infrastructure.Entities.RegisterUnitToken", b =>
                {
                    b.Property<Guid>("TokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountId");

                    b.Property<string>("CreatorUserName")
                        .HasMaxLength(50);

                    b.Property<Guid?>("SiteId");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("UnitId");

                    b.Property<string>("UserId");

                    b.HasKey("TokenId");

                    b.ToTable("RegisterUnitTokens");
                });

            modelBuilder.Entity("Infrastructure.Entities.ReportSchedule", b =>
                {
                    b.Property<Guid>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailList")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("LastSent")
                        .HasColumnType("datetime");

                    b.Property<string>("RecurrenceRule")
                        .HasMaxLength(128);

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("ReportPeriod")
                        .HasColumnType("tinyint");

                    b.Property<string>("ScheduleTitle")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ScheduleId");

                    b.ToTable("ReportSchedules");
                });

            modelBuilder.Entity("Infrastructure.Entities.ReportScheduleSite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ReportScheduleSites");
                });

            modelBuilder.Entity("Infrastructure.Entities.Result", b =>
                {
                    b.Property<Guid>("ResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ActualIncubationTime")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ControlExpirationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ControlLotNumber")
                        .HasMaxLength(25);

                    b.Property<string>("ControlModifiedBy")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ControlModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ControlName")
                        .HasMaxLength(25);

                    b.Property<Guid?>("CorrectedTest")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CorrectiveAction")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CustomField1")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField10")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField2")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField3")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField4")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField5")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField6")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField7")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField8")
                        .HasMaxLength(4000);

                    b.Property<string>("CustomField9")
                        .HasMaxLength(4000);

                    b.Property<string>("DeviceCategory")
                        .HasMaxLength(10);

                    b.Property<string>("DeviceName")
                        .HasMaxLength(50);

                    b.Property<int?>("DeviceTemperature")
                        .HasColumnType("int");

                    b.Property<string>("DeviceUOM")
                        .HasMaxLength(10);

                    b.Property<int?>("Dilution")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .HasMaxLength(250);

                    b.Property<int?>("IncubationTime")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("Lower")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Notes")
                        .HasMaxLength(250);

                    b.Property<string>("Personnel")
                        .HasMaxLength(256);

                    b.Property<string>("PlanName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("RLU")
                        .HasColumnType("int");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("RawADCOutput")
                        .HasColumnType("int");

                    b.Property<byte?>("RepeatedTest")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("ResultDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("RoomNumber")
                        .HasColumnType("int");

                    b.Property<string>("SampleType")
                        .HasMaxLength(25);

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SurfaceName")
                        .HasMaxLength(250);

                    b.Property<byte?>("TestState")
                        .HasColumnType("tinyint");

                    b.Property<int?>("UnitAngle")
                        .HasColumnType("int");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("UnitNo")
                        .HasColumnType("int");

                    b.Property<string>("UnitSoftware")
                        .HasMaxLength(10);

                    b.Property<string>("UnitType")
                        .HasMaxLength(100);

                    b.Property<int?>("Upper")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(30);

                    b.Property<int>("WarningId")
                        .HasColumnType("int");

                    b.Property<string>("Zone")
                        .HasMaxLength(4000);

                    b.HasKey("ResultId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Infrastructure.Entities.Site", b =>
                {
                    b.Property<Guid>("SiteId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccountId");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("SiteId");

                    b.HasIndex("AccountId");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Infrastructure.Entities.SiteUser", b =>
                {
                    b.Property<Guid>("SiteUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SiteId");

                    b.Property<Guid?>("UserId");

                    b.HasKey("SiteUserId");

                    b.ToTable("SiteUsers");
                });

            modelBuilder.Entity("Infrastructure.Entities.SureTrendVersionInfo", b =>
                {
                    b.Property<string>("AppVersion")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("DatabaseVersion")
                        .HasMaxLength(100);

                    b.HasKey("AppVersion");

                    b.ToTable("SureTrendVersionInfo");
                });

            modelBuilder.Entity("Infrastructure.Entities.Surface", b =>
                {
                    b.Property<Guid>("SurfaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Notes")
                        .HasMaxLength(4000);

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("SurfaceName")
                        .HasMaxLength(250);

                    b.HasKey("SurfaceId");

                    b.ToTable("Surfaces");
                });

            modelBuilder.Entity("Infrastructure.Entities.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(250);

                    b.Property<Guid>("AccountId");

                    b.Property<Guid>("SiteId");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("UnitNo");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Infrastructure.Entities.UnitLocationMap", b =>
                {
                    b.Property<int>("UnitNo")
                        .HasColumnType("int");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.HasKey("UnitNo", "LocationId");

                    b.HasAlternateKey("LocationId", "UnitNo");

                    b.ToTable("UnitLocationMap");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("UserAccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccountId");

                    b.Property<Guid>("UserId");

                    b.HasKey("UserAccountId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserFunctionRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountId");

                    b.Property<string>("Role");

                    b.Property<string>("RoleDescription");

                    b.HasKey("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserRolePermission", b =>
                {
                    b.Property<Guid>("RoleUserPermissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PermissionId");

                    b.Property<Guid?>("RoleId");

                    b.HasKey("RoleUserPermissionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRolePermissions");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserSetting", b =>
                {
                    b.Property<Guid>("UserSettingId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DashDateFrom")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DashDateTo")
                        .HasColumnType("datetime");

                    b.Property<byte?>("DashPeriod")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("DashType")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("LanguageId");

                    b.Property<string>("ReportScheduleCurrentView")
                        .HasMaxLength(25);

                    b.Property<string>("ResultGridSchema");

                    b.Property<DateTime?>("ResultsDateFrom")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("ResultsDateTo")
                        .HasColumnType("datetime");

                    b.Property<byte?>("ResultsPeriod")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId");

                    b.HasKey("UserSettingId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Infrastructure.UserModel.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<Guid?>("UserRoleRoleId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("UserRoleRoleId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Infrastructure.Entities.LanguageUserValue", b =>
                {
                    b.HasOne("Infrastructure.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("Infrastructure.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId");

                    b.HasOne("Infrastructure.Entities.LanguageResource", "LanguageResource")
                        .WithMany()
                        .HasForeignKey("LanguageResourceResourceValueId");
                });

            modelBuilder.Entity("Infrastructure.Entities.LanguageValue", b =>
                {
                    b.HasOne("Infrastructure.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId");

                    b.HasOne("Infrastructure.Entities.LanguageResource", "LanguageResource")
                        .WithMany()
                        .HasForeignKey("LanguageResourceResourceValueId");
                });

            modelBuilder.Entity("Infrastructure.Entities.Location", b =>
                {
                    b.HasOne("Infrastructure.Entities.Surface")
                        .WithMany("Locations")
                        .HasForeignKey("SurfaceId");
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanHistoryResult", b =>
                {
                    b.HasOne("Infrastructure.Entities.PlanHistory")
                        .WithMany("PlanHistoryResults")
                        .HasForeignKey("PlanHistoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanLocation", b =>
                {
                    b.HasOne("Infrastructure.Entities.Plan")
                        .WithMany("PlanLocations")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanLocationMap", b =>
                {
                    b.HasOne("Infrastructure.Entities.Location")
                        .WithMany("PlanLocationMaps")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.PlanUnit", b =>
                {
                    b.HasOne("Infrastructure.Entities.Plan")
                        .WithMany("PlanUnits")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.Site", b =>
                {
                    b.HasOne("Infrastructure.Entities.Account")
                        .WithMany("Sites")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.UnitLocationMap", b =>
                {
                    b.HasOne("Infrastructure.Entities.Location")
                        .WithMany("UnitLocationMaps")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.UserRolePermission", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("Infrastructure.Entities.UserFunctionRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserSetting", b =>
                {
                    b.HasOne("Infrastructure.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId");

                    b.HasOne("Infrastructure.UserModel.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Infrastructure.UserModel.Models.ApplicationUser", b =>
                {
                    b.HasOne("Infrastructure.Entities.UserFunctionRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleRoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Infrastructure.UserModel.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Infrastructure.UserModel.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.UserModel.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
