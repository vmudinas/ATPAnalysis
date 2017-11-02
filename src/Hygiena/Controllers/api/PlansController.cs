using System;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [Authorize(Roles = "plansViewEnabled")]
    public class PlansController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public PlansController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("GetPlans")]
        public IActionResult GetPlans()
        {
            try
            {
                //User.Identity.Name
                return Ok(_dataService.GetPlans(User?.Identity?.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "GetLocations");
                return StatusCode(500);
            }
        }
    }
}