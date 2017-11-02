using System;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Models.ManageViewModels;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    public class ManagementController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IDataService _dataService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagementController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender, IDataService dataService,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<ManagementController>();
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _config = configuration;
        }

        #region User Password Change

        [Route("ChangePassword")]
        [HttpPost]
        // [Authorize(Roles = "passwordChangeEnabled")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (user != null)
                {
                    var result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;
                    if (result.Succeeded)
                    {
                        _signInManager.SignInAsync(user, false);
                        _logger.LogInformation(3, "User changed their password successfully.");
                        return Ok("Success");
                    }

                    return Ok(result.Errors);
                }
                return Ok(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return Ok(0);
            }
        }

        #endregion
    }
}