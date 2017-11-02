using System;
using DevExtreme.AspNet.Data;
using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    public class ResultController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public ResultController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("Result")]
        [Authorize(Roles = "resultViewEnabled")]
        public IActionResult GetResults(string fromUtc, string toUtc)
        {
            try
            {
                return
                    Ok(_dataService.GetResults(DateTimeConverter.MomentUtcToDateTime(fromUtc),
                        DateTimeConverter.MomentUtcToDateTime(toUtc),
                        _dataService.GetCurrentUserAccount(User?.Identity?.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"GetResults Failed: {User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        [Route("PagedResults")]
        [Authorize(Roles = "resultViewEnabled")]
        public IActionResult GetPagedResults(string fromUtc, string toUtc, string loadOption)
        {
            try
            {
                var options = JsonConvert.DeserializeObject<DataSourceLoadOptions>(loadOption);
                var data = _dataService.GetPagedResults(DateTimeConverter.MomentUtcToDateTime(fromUtc),
                    DateTimeConverter.MomentUtcToDateTime(toUtc), options,
                    _dataService.GetCurrentUserAccount(User?.Identity?.Name));

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"PagedResults Failed: {User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        [Route("PagedResultsCount")]
        [Authorize(Roles = "resultViewEnabled")]
        public IActionResult GetPagedCountResults(string fromUtc, string toUtc, string loadOption)
        {
            try
            {
                var options = JsonConvert.DeserializeObject<DataSourceLoadOptions>(loadOption);

                return
                    Ok(_dataService.GetPagedResultsCount(DateTimeConverter.MomentUtcToDateTime(fromUtc),
                        DateTimeConverter.MomentUtcToDateTime(toUtc), options,
                        _dataService.GetCurrentUserAccount(User?.Identity?.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"GetPagedCountResults Failed: {User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        [Route("UpdateResult")]
        [Authorize(Roles = "resultViewEnabled")]
        public IActionResult UpdateResult(ClientResult cr)
        {
            try
            {
                _dataService.UpdateResult(cr);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"UpdateResult Failed: {User?.Identity?.Name}");
                return StatusCode(500);
            }
        }
    }
}