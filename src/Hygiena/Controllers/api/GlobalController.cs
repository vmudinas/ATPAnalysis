using System;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [AllowAnonymous]
    public class GlobalController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public GlobalController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("GetUtc")]
        [AllowAnonymous]
        public IActionResult GetUtc()
        {
            try
            {
                _dataService.MigrationLanguages();
                return Ok(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(1,ex, $"GetLocations : {User?.Identity?.Name}");
                return StatusCode(500);
            }
        }
     

        [Route("AllowDebug")]
        [AllowAnonymous]
        public IActionResult AllowDebug(string token, string unitSerial)
        {
            try
            {
                return Ok(_dataService.ExpireToken(token, unitSerial));

            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

    }
}