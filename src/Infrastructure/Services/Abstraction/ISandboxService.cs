//using System;
//using Infrastructure.ClientEntities;

using System.Collections.Generic;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.UnitEntities;

namespace Infrastructure.Services.Abstraction
{
    public interface ISandboxService
    {
        IEnumerable<MongoResult> GetAllResults();
        IEnumerable<MongoResult> GetResultsByUnit(int unitNo);
        IEnumerable<ClientUnitResult> AddTestResults(ResultsItem unitResultsItem);
    }
}