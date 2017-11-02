using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DataContext;
using Infrastructure.Services;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HygienUnitTests
{
    public class UnitPlanTest
    {

        private IDataService _service;
        public UnitPlanTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer("Server=tcp:zvp4bnjcrg.database.windows.net,1433;Initial Catalog=SureTrendDev;Persist Security Info=False;User ID=hygiena;Password=SystemSURE2$#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var context = new DatabaseContext(builder.Options);
            var nodeServices = NodeServicesFactory.CreateNodeServices(new NodeServicesOptions(serviceProvider));
            IEmailSender emailSender = new AuthMessageSender();
            _service = new DataService(context, nodeServices, emailSender, null,null,null);

        }


        [Theory]
        [InlineData(1)]
        public void TestAddUnitPlan(int i)
        {
            var unitNo1 = 11111;
            var unitNo2 = 22222;
            var acctId = Guid.NewGuid();
            var siteId = Guid.NewGuid();
            var planId1 = Guid.NewGuid();
            var planId2 = Guid.NewGuid();
            var planId3 = Guid.NewGuid();
            var loc1 = Guid.NewGuid();
            var loc2 = Guid.NewGuid();
            var loc3 = Guid.NewGuid();
            var unitPlan = new UnitPlan();
            var lstUnitPlan = new List<UnitPlan>();
            var lstLocs = new List<Guid>();
            var lstLocsReqd = new List<Guid>();

            lstLocs.Add(loc1);
            lstLocs.Add(loc2);
            lstLocs.Add(loc3);
            lstLocsReqd.Add(loc2);

            // EnSURE5 units have 3 types of plans (in increasing complexity): Basic, Quota & Random
            
            // Random plan
            lstUnitPlan.Add(new UnitPlan()
            {
                IsRandom = true,
                Locations = lstLocs,
                ModifiedBy = "Random Cindy",
                ModifiedDate = DateTime.UtcNow.AddHours(-6),
                PlanId = planId1,
                PlanName = "Mixers",
                Quota = 2,
                RequiredLocations = lstLocsReqd,
                ShouldPreventRepeat = true
            });

            lstLocsReqd.Clear();
            lstLocs.Clear();
            lstLocs.Add(loc1);
            lstLocs.Add(loc2);
            lstLocs.Add(loc3);

            // Quota plan
            lstUnitPlan.Add(new UnitPlan()
            {
                IsRandom = false,
                Locations = lstLocs,
                ModifiedBy = "Quota Linda",
                ModifiedDate = DateTime.UtcNow.AddHours(-6),
                PlanId = planId2,
                PlanName = "Conveyer Belts",
                Quota = 2,
                RequiredLocations = lstLocsReqd,
                ShouldPreventRepeat = false
            });

            lstLocs.Clear();
            lstLocs.Add(loc1);
            lstLocs.Add(loc3);

            // Basic plan
            lstUnitPlan.Add(new UnitPlan()
            {
                IsRandom = false,
                Locations = lstLocs,
                ModifiedBy = "Basic Joe",
                ModifiedDate = DateTime.UtcNow.AddHours(-6),
                PlanId = planId3,
                PlanName = "Tube Feeders",
                Quota = 0,
                RequiredLocations = lstLocsReqd,
                ShouldPreventRepeat = false
            });

            var piUnit = new PlanItem() { AccountId = acctId, SiteId = siteId, UnitNo = unitNo1, Payload = lstUnitPlan };

            _service.AddPlansFromUnit(piUnit); // should insert 3 plans (for unit 11111): Basic, Quota & Random

            // Verify existence of (above 3) added plans
            Assert.Equal(3, _service.GetPlanCountFromPlanIdList(new List<Guid> { planId1, planId2, planId3 }));

            // Add a plan with another unit number to verify that PlanUnit table gets more than one record per plan
            lstUnitPlan.Clear();

            // Basic plan for a different unit (i.e. Unit2). Should insert more rows to PlanUnit table for plan3.
            lstUnitPlan.Add(new UnitPlan()
            {
                IsRandom = false,
                Locations = lstLocs,
                ModifiedBy = "Basic David",
                ModifiedDate = DateTime.UtcNow.AddHours(-6),
                PlanId = planId3,
                PlanName = "Tube Feeders",
                Quota = 0,
                RequiredLocations = lstLocsReqd,
                ShouldPreventRepeat = false
            });

            piUnit = new PlanItem() { AccountId = acctId, SiteId = siteId, UnitNo = unitNo2, Payload = lstUnitPlan };

            _service.AddPlansFromUnit(piUnit);

            // Change modified by from "Basic Joe" to "Basic Joseph" planId3 and use only one location
            lstLocs.Clear();
            lstLocs.Add(loc1);
            lstUnitPlan.Clear();

            // Basic plan changed
            lstUnitPlan.Add(new UnitPlan()
            {
                IsRandom = false,
                Locations = lstLocs,
                ModifiedBy = "Basic Joseph", // the next "Assert" will detect that "Basic Joseph" is the new ModifiedBy for plan 3
                ModifiedDate = DateTime.UtcNow.AddHours(-1),
                PlanId = planId3,
                PlanName = "Tube Feeders",
                Quota = 0,
                RequiredLocations = lstLocsReqd,
                ShouldPreventRepeat = false
            });

            piUnit = new PlanItem() { AccountId = acctId, SiteId = siteId, UnitNo = unitNo1, Payload = lstUnitPlan };

            _service.AddPlansFromUnit(piUnit); // if plan id exists, AddPlansFromUnit (should update) it, else insert

            // assert existence of changed plan
            Assert.Equal(planId3, _service.GetPlanIdByIdAndModBy(planId3, "Basic Joseph")); // expecting plan3 to return

            // Remove unit test plans 1, 2 & 3
            _service.DeletePlanList(new List<Guid> { planId1, planId2, planId3 });
        }

        [Theory]
        [InlineData(1)]
        public void TestGetSureTrendPlan(int i)
        {
            var acctId = Guid.Parse("02380FA3-1AC6-42A5-9F24-F1B45C285F90");
            const int unitNo = 15008634;
            var lstPlanPayload = _service.GetPlanPayload(acctId, unitNo);

            Assert.True(lstPlanPayload != null);
        }

    }
}
