using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Moq;
using Xunit;
using Infrastructure.UnitEntities;

namespace HygienUnitTests.Services
{
    public class SandBoxServiceHomeTests
    {

        [Fact]
        public void TestGetFailsByLocation()
        {


            var dataSource = new Mock<ISandboxService>();
            var resultList = new List<UnitResult>();
            var unitResult = new ResultsItem();

            resultList.Add(
                new UnitResult
                    {
                        ResultId = new Guid("26c0f6f1-0643-43db-8d45-d9984bbffb5f"),
                        AccountId = new Guid("02380FA3-1AC6-42A5-9F24-F1B45C285F90"),
                        ActualIncubationTime = null,
                        ControlExpirationDate = null,
                    ControlLotNumber = null,
                    ControlModifiedBy = null,
                    ControlModifiedDate = null,
                    ControlName = null,
                    CorrectedTest = null,
                    CorrectiveAction = null,
                    CreatedBy = null,
                    CreatedDate = null,
                        CustomField1 = null,
                    CustomField2 = null,
                    CustomField3 = null,
                    CustomField4 = null,
                    CustomField5 = null,
                    CustomField6 = null,
                    CustomField7 = null,
                    CustomField8 = null,
                    CustomField9 = null,
                    CustomField10 = null,
                    DeviceCategory = null,
                    DeviceName = null,
                    DeviceTemperature = null,
                    DeviceUOM = null,
                    Dilution = null,
                    GroupName = null,
                    IncubationTime = null,
                    IsDeleted = null,
                    LocationName = null,
                    Lower = null,
                    ModifiedBy = null,
                    ModifiedDate = null,
                    Notes = null,
                    Personnel = null,
                    Rank = null,
                    RawADCOutput = 0,
                    RepeatedTest = null,
                    //  retRes.Result = urIn.Result;
                    ResultDate = DateTime.Now,
                   // ResultId = new Guid("26c0f6f1-0643-43db-8d45-d9984bbffb5f"),
                    RLU = 3,
                    RoomNumber = null, 
                    SampleType = null,
                    SiteId = new Guid("3BFC272A-9A7F-427F-831A-D44E57B4F1EB"),
                        SurfaceName = null,
                    PlanName = null,
                    TestState = null,
                    UnitAngle = null,
                    UnitName = null,
                    UnitNo = 15008634,
                        UnitSoftware = null,
                    UnitType = null,
                    Upper = null,
                    UserName = null,
                    WarningId = 0,
                    Zone = null
                }
                    ); 
           


            var unitResult2 = new ResultsItem
            {
               AccountId = new Guid("02380FA3-1AC6-42A5-9F24-F1B45C285F90"),
               SiteId = new Guid("3BFC272A-9A7F-427F-831A-D44E57B4F1EB"),
               UnitNo = 15008634,
               Payload = resultList
              

            };
         

        ///    dataSource.Setup(m => m.AddTestResults(unitResult2)).Returns(resultList);

        

        }

     //   public IEnumerable<Result> AddTestResults(ResultsItem unitResultsItem)
  



    }
}