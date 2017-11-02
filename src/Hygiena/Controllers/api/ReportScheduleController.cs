using System;
using Infrastructure.ClientEntities;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    //[Authorize(Roles = "canScheduleEmail")] // moved below to individual classes since want to allow Web Job to call email sender
    public class ReportScheduleController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public ReportScheduleController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("GetReportSchedules")]
        [Authorize(Roles = "canScheduleEmail")]
        public IActionResult GetReportSchedules()
        {
            try
            {
                return Ok(_dataService.GetReportSchedules(User?.Identity?.Name ?? ""));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} : GetReportSchedules");
                return StatusCode(500);
            }
        }

        [Route("UpdateReportSchedule")]
        [Authorize(Roles = "canScheduleEmail")]
        public IActionResult UpdateReportSchedule(ClientReportSchedule crs)
        {
            try
            {
                _dataService.UpdateReportSchedule(crs, User?.Identity?.Name ?? "");
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "UpdateReportSchedule");
                return StatusCode(500);
            }
        }

        [Route("AddReportSchedule")]
        [Authorize(Roles = "canScheduleEmail")]
        public IActionResult AddReportSchedule(ClientReportSchedule crs)
        {
            try
            {
                _dataService.AddReportSchedule(crs, User.Identity?.Name ?? "");
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "AddReportSchedule");
                return StatusCode(500);
            }
        }

        [Route("DeleteReportSchedule")]
        [Authorize(Roles = "canScheduleEmail")]
        public IActionResult DeleteReportSchedule(string accountId, string scheduleId)
        {
            try
            {
                _dataService.DeleteReportSchedule(accountId, scheduleId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "DeleteReportSchedule");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("SendScheduledReports")]
        public IActionResult SendScheduledReports()
        {
            try
            {
                _dataService.SendScheduledReports();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "SendScheduledReports");
                return StatusCode(500);
            }
        }
    }
}