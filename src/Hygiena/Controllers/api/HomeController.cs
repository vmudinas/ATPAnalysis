using System;
using System.Threading.Tasks;
using Infrastructure.BusinessLogic;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [Authorize(Roles = "homeViewEnabled")]
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;
        
        public HomeController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }



        [Route("GetAllHomeData")]
        public IActionResult GetAllHomeData(string fromUtc, string toUtc)
        {
            try
            {
                return Ok(_dataService.GetAllHomeData(DateTimeConverter.MomentUtcToDateTime(fromUtc),
                    DateTimeConverter.MomentUtcToDateTime(toUtc),
                    _dataService.GetCurrentUserAccount(User?.Identity?.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} allResults");
                return StatusCode(500);
            }
        }

     
    }
}