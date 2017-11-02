using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.Services.Abstraction;
using Moq;
using Xunit;

namespace HygienUnitTests.Services
{

    public class DataServiceHomeTests
    {
      
        [Fact]
        public void TestGetFailsByLocation()
        {
            
            var service = CreateService();

            //     service.Setup(m => m.GetAllResults()).Returns(ReturnResultData());
            var account = new Account
            {
                AccountId = Guid.NewGuid()
            };
            var testResults = service.Object.GetAllResults(account).ToList();
             var failsByLocationLogic = new DataServiceHomeLogic();
             var result = failsByLocationLogic.ReturnFailsChartLogic(testResults.ToList());

            // GetFailsByLocation(DateTime fromUtc, DateTime toUtc)

        }

        private static Mock<IDataService> CreateService()
        {
            return new Mock<IDataService>();
          
        }

        private static IEnumerable<MongoResult> ReturnResultData(string locationName, string locationName2, string locationName3)
        {
           var results =  new List<MongoResult>();
            var firstResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                LocationName = locationName
            };
            var secondResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                LocationName = locationName2
            };
            var thirdResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                LocationName = locationName3
            };
            results.Add(firstResult);
            results.Add(secondResult);
            results.Add(thirdResult);

            return results;
        }


    
    }
}