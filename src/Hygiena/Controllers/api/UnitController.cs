using System;
using Infrastructure.ClientEntities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
  // [Authorize("Bearer")]
    [AllowAnonymous]


    public class UnitController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public UnitController(ILoggerFactory loggerFactory, IDataService dataService)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();
            _dataService = dataService;
        }

        [Route("GetUnitLatestVersion")]
        public IActionResult GetAllResults()
        {
            try
            {
                var unitVersino = new UnitVersion {Version = 2};
                return Ok(unitVersino);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetUnitVersion error:", ex);

                return StatusCode(500);
            }
        }

        [Route("DownloadUnitVersion")]
        public IActionResult DownloadUnitVersion(int version)
        {
            try
            {
                var fileName = $"ensure5-v2.apk";
                var filepath = $"wwwroot/unitVersions/{fileName}";
                var fileBytes = System.IO.File.ReadAllBytes(filepath);
                return File(fileBytes, "application/x-msdownload", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetUnitVersion error:", ex);

                return StatusCode(500);
            }
        }

        [Route("AddResults")]
        [HttpPost]
        public IActionResult AddResults([FromBody] ResultsItem unitResultsItem)
        {
            if (unitResultsItem == null || unitResultsItem.UnitNo < 0 || unitResultsItem.Payload == null
                || unitResultsItem.AccountId == Guid.Empty || unitResultsItem.SiteId == Guid.Empty)
                return StatusCode(520);

            try
            {
                return Ok(_dataService.AddTestResults(unitResultsItem));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex?.Message);
                _logger.LogCritical(ex?.InnerException?.ToString());
                return StatusCode(500);
            }
        }
        [Route("AddResultsCloudSync")]
        [HttpPost]
        public IActionResult AddResultsCloudSync([FromBody] ClientCloudSyncResult syncData)
        {
            if (syncData?.Payload == null || syncData.AccountId == Guid.Empty || syncData.SiteId == Guid.Empty)
                return StatusCode(520);

            try
            {
                return Ok(_dataService.AddResultCloudSyncMongo(syncData));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex?.Message);
                _logger.LogCritical(ex?.InnerException?.ToString());
                return StatusCode(500);
            }
        }

        [Route("AddPlans")]
        [HttpPost]
        public IActionResult AddPlans([FromBody] PlanItem unitPlanItem)
        {
            if (unitPlanItem == null || unitPlanItem.UnitNo < 0 || unitPlanItem.Payload == null
                || unitPlanItem.AccountId == Guid.Empty || unitPlanItem.SiteId == Guid.Empty)
            { return StatusCode(520); }

            try
            {
                return Ok(_dataService.AddPlansFromUnit(unitPlanItem));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex?.Message);
                _logger.LogCritical(ex?.InnerException?.ToString());
                return StatusCode(500);
            }
        }

        [Route("GetPlans")]
        [HttpPost]
        public IActionResult GetPlans([FromBody] PlanItem unitPlanItem)
        {
            if (unitPlanItem == null || unitPlanItem.UnitNo < 0 || unitPlanItem.AccountId == Guid.Empty || unitPlanItem.SiteId == Guid.Empty)
            { return StatusCode(520); }

            try
            {
                return Ok(_dataService.GetPlanPayload(unitPlanItem.AccountId, unitPlanItem.UnitNo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex?.Message);
                _logger.LogCritical(ex?.InnerException?.ToString());
                return StatusCode(500);
            }
        }
    }
}