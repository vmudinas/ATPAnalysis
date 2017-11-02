using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.BusinessLogic;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Tls;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Server.Kestrel;
using System.Collections.Generic;
using Infrastructure.UserModel.Models;
using Microsoft.AspNetCore.Identity;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [AllowAnonymous]
    public class ReportsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public ReportsController(IDataService dataService, ILoggerFactory loggerFactory,
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [Route("EmailReport")]
        [HttpGet]
        [AllowAnonymous]
       
        public IActionResult EmailReport(string userName,string title, string reportType, string fromUtc, string toUtc, List<string> emailList)
        {
            try
            {

                var user = _userManager.FindByNameAsync(userName).Result;

                if (user != null)
                {
                    _signInManager.SignInAsync(user, true);
                }

                var result = _dataService.RunNodeReport(DateTimeConverter.MomentUtcToDateTime(fromUtc),
                      DateTimeConverter.MomentUtcToDateTime(toUtc), userName, title, reportType, emailList);

                _signInManager?.SignOutAsync();

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"{userName} generateReport");

                return Ok(ex);
            }
        }

        [Route("GetReportData")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetReportData( string fromUtc, string toUtc)
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
    }
}