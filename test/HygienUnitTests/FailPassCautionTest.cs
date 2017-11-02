using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.Services.Abstraction;
using Moq;
using Xunit;

namespace HygienUnitTests
{
    public class FailPassCautionTest
    {
        [Theory]
        [InlineData(1,1,1, 1, 1, 1, 1, 1, 1,3,0,0)]
        [InlineData(5, 7, 10, 5, 3,4, 8, 7, 8, 1, 1, 1)]
        public void PassFailsChartLogicTest(
            int firstRlu, int firstLower, int firstUpper,
            int secondRlu, int secondLower, int secondUpper,
            int thirdRlu, int thirdLower, int thirdUpper,
            int passResult, int cautionResult, int failResult
            )
        {
            var dataSource = new Mock<IDataService>();
            var resultList = new List<MongoResult>();
            var firstResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = firstRlu,
                Lower = firstLower,
                Upper = firstUpper
            };
            var secondResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = secondRlu,
                Lower = secondLower,
                Upper = secondUpper
            };
            var thirdResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = thirdRlu,
                Lower = thirdLower,
                Upper = thirdUpper
            };
            resultList.Add(firstResult);
            resultList.Add(secondResult);
            resultList.Add(thirdResult);

            var account = new Account
            {
                AccountId = Guid.NewGuid()
            };
            

            dataSource.Setup(m => m.GetAllResults(account)).Returns(resultList);

          
            var failtChartLogit = new FailsChartLogic();
           var result =  failtChartLogit.ReturnFailsChartLogic(dataSource.Object.GetAllResults(account).ToList());

            Assert.Equal(passResult,result?.FirstOrDefault(x=>x.Result == "Pass")?.Area);
            Assert.Equal(cautionResult, result?.FirstOrDefault(x => x.Result == "Caution")?.Area);
            Assert.Equal(failResult, result?.FirstOrDefault(x => x.Result == "Fail")?.Area);

        }
        [Theory]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,1)]
        [InlineData(5, 7, 10, 5, 3, 4, 8, 7, 8, 3, 0, 0)]
        public void FailFailsChartLogicTest(
           int firstRlu, int firstLower, int firstUpper,
           int secondRlu, int secondLower, int secondUpper,
           int thirdRlu, int thirdLower, int thirdUpper,
           int passResult, int cautionResult, int failResult
           )
        {
            var dataSource = new Mock<IDataService>();
            var resultList = new List<MongoResult>();
            var firstResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = firstRlu,
                Lower = firstLower,
                Upper = firstUpper
            };
            var secondResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = secondRlu,
                Lower = secondLower,
                Upper = secondUpper
            };
            var thirdResult = new MongoResult
            {
                ResultId = Guid.NewGuid().ToString(),
                RLU = thirdRlu,
                Lower = thirdLower,
                Upper = thirdUpper
            };
            resultList.Add(firstResult);
            resultList.Add(secondResult);
            resultList.Add(thirdResult);

            var account = new Account
            {
                AccountId = Guid.NewGuid()
            };

            dataSource.Setup(m => m.GetAllResults(account)).Returns(resultList);


            var failtChartLogit = new FailsChartLogic();
            var result = failtChartLogit.ReturnFailsChartLogic(dataSource.Object.GetAllResults(account).ToList());

            Assert.NotEqual(passResult, result?.FirstOrDefault(x => x.Result == "Pass").Area);
            Assert.NotEqual(cautionResult, result?.FirstOrDefault(x => x.Result == "Caution").Area);
            Assert.NotEqual(failResult, result?.FirstOrDefault(x => x.Result == "Fail").Area);
        }
    }
}