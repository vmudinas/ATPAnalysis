using System;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [Authorize(Roles = "locationsViewEnabled")]
    public class LocationsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public LocationsController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("GetLocations")]
        public IActionResult GetLocations()
        {
            try
            {
                //User.Identity.Name
                return Ok(_dataService.GetLocations(User?.Identity?.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "GetLocations");
                return StatusCode(500);
            }
        }
    }
}