using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Models.ManageViewModels;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    [Authorize(Roles = "adminViewEnabled")]
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IDataService _dataService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender, IDataService dataService,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _config = configuration;
        }

        [Route("GetCurrentUser")]
        public IActionResult GetAllCurrentAccountUser()
        {
            return Ok(User?.Identity?.Name);
        }

        [Route("GetCurrentUsers")]
        public IActionResult GetAllCurrentAccountUsers()
        {
            try
            {
                return Ok(_dataService.GetAllCurrentAccountUsers(User?.Identity?.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetCurrentUsers");
                return StatusCode(500);
            }
        }

        [Route("GetUserSettings")]
        public IActionResult GetUserSettings()
        {
            try
            {
                return Ok(_dataService.GetUserSettings(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetUserSettings");
                return StatusCode(500);
            }
        }

        [Route("UpdateUserSettings")]
        public IActionResult UpdateUserSettings(string whichSection, ClientUserSetting cus, string fromUtc, string toUtc)
        {
            try
            {
                _dataService.UpdateUserSettings(User.Identity.Name, whichSection, cus,
                    DateTimeConverter.MomentUtcToDateTime(fromUtc), DateTimeConverter.MomentUtcToDateTime(toUtc));
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} UpdateUserSettings");
                return StatusCode(500);
            }
        }

        [Route("GetAllRolePermissions")]
        public IActionResult GetAllRolePermissions()
        {
            try
            {
                return Ok(_dataService.GetAllRolePermissions());
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetAllRolePermissions");
                return StatusCode(500);
            }
        }

        [Route("GetUserRolePermissions")]
        public IActionResult GetUserRolePermissions()
        {
            try
            {
                return Ok(_dataService.GetUserRolePermissions(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetUserRolePermissions");
                return StatusCode(500);
            }
        }

        [Route("GetRolePermissions")]
        public IActionResult GetRolePermissions(string role)
        {
            try
            {
                return Ok(_dataService.GetRolePermissions(role));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetRolePermissions");
                return StatusCode(500);
            }
        }

        [Route("GetUserAccount")]
        public IActionResult GetCurrentUserAccount()
        {
            try
            {
                return Ok(_dataService.GetCurrentUserAccount(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetUserAccount");
                return StatusCode(500);
            }
        }

        [Route("GetAccountSites")]
        public IActionResult GetAccountSite()
        {
            try
            {
                return Ok(_dataService.GetAllSitesForAccount(User?.Identity?.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetAccountSite");
                return StatusCode(500);
            }
        }

        [Route("AddUser")]
        public IActionResult AddUser(string userName, string email, string role)
        {
            try
            {
                var account = _dataService.GetCurrentUserAccount(User.Identity.Name);
                var user = new UserManagement { UserName = userName, Email = email, RoleName = role };

                _dataService.AddUser(account, user);
                //After User is added send an email with password reset 
                var userReset = _userManager.FindByEmailAsync(email).Result;
                if (userReset == null) // || !(await _userManager.IsEmailConfirmedAsync(user)))
                    return View("ForgotPasswordConfirmation");


                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = _userManager.GeneratePasswordResetTokenAsync(userReset).Result;
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userReset.Id, code },
                    HttpContext.Request.Scheme);
                _emailSender.SendEmailAsync(email, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>",
                    _config.GetSection("EmailUserName").Value,
                    _config.GetSection("EmailAddress").Value,
                    _config.GetSection("EmailPassword").Value,
                    _config.GetSection("SmtpServer").Value,
                    int.Parse(_config.GetSection("SmtpServerPort").Value));

                return Ok("Done");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} AddUser");
                return StatusCode(500);
            }
        }

        [Route("UpdateUser")]
        // oldUser: oldUserName, newUser: newUser, newEmail: newEmail, newRole: newRole
        public IActionResult UpdateUser(string oldUser, string newUser, string newEmail, string newRole)

        {
            try
            {
                var account = _dataService.GetCurrentUserAccount(User.Identity.Name);
                var user = new UserManagement
                {
                    Email = newEmail,
                    RoleName = newRole,
                    UserName = newUser ?? oldUser
                };
                _dataService.UpdateUser(account, user, oldUser);
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} UpdateUser");
                return StatusCode(500);
            }
        }

        [Route("RemoveUser")]
        public IActionResult RemoveUser(string userName)
        {
            try
            {
                _dataService.RemoveUser(userName);
                return Ok("Removed");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} RemoveUser");
                return StatusCode(500);
            }
        }

        [Route("GetAccountRoles")]
        public IActionResult GetAccountRoles()
        {
            try
            {
                return Ok(_dataService.GetAllCurrentAccountRoles(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} GetAccountRoles");
                return StatusCode(500);
            }
        }

        [Route("AddRole")]
        public IActionResult AddRole(Guid roleId, string role, string roleDescription)
        {
            try
            {
                var account = _dataService.GetCurrentUserAccount(User.Identity.Name);
                _dataService.AddRole(roleId, role, roleDescription, account);
                return Ok("Role Added");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} AddRole");
                return StatusCode(500);
            }
        }

        [Route("UpdateRole")]
        public IActionResult UpdateRole(string roleId, string role, string roleDescription)
        {
            try
            {
                _dataService.UpdateRole(roleId, role, roleDescription);
                return Ok("Updated role");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} UpdateRole");
                return StatusCode(500);
            }
        }

        [Route("RemoveRole")]
        public IActionResult RemoveRole(string roleId)
        {
            try
            {
                _dataService.RemoveRole(roleId);
                return Ok("Role removed");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} RemoveRole");
                return StatusCode(500);
            }
        }

        #region User Password Change

        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (user == null) return Ok(0);
                var result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;
                if (!result.Succeeded) return Ok(1);
                _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User changed their password successfully.");
                return Ok(3);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        #endregion

        #region Permission Management

        [Route("AddPermission")]
        public IActionResult AddPermission(string role, string permission, bool status)
        {
            try
            {
                var account = _dataService.GetCurrentUserAccount(User.Identity.Name);
                if (status)
                    _dataService.RemovePermission(role, permission, account);
                else
                    _dataService.AddPermission(role, permission, account);

                return Ok("Permission Added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "GetAccountSites");
                return StatusCode(500);
            }
        }

        [Route("RemovePermission")]
        public IActionResult RemovePermission(string rolePermissionId)
        {
            try
            {
                //   _dataService.RemoveRole(rolePermissionId);
                return Ok("Role removed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "GetAccountSites");
                return StatusCode(500);
            }
        }

        #endregion

        #region Settings Management

        [Route("UpdateResultGridSchema")]
        public IActionResult UpdateResultGridSchema(string resultGridSchemaJson)
        {
            try
            {
                _dataService.UpdateResultGridSchema(User.Identity.Name, resultGridSchemaJson);
                return Ok("Result Grid Schema Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message,
                    User?.Identity?.Name ?? "UpdateResultGridSchema");
                return StatusCode(500);
            }
        }

        [Route("GetResultGridSchema")]
        public IActionResult GetResultGridSchema()
        {
            try
            {
                return Ok(_dataService.GetResultGridSchema(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message, ex.Message, User?.Identity?.Name ?? "GetResultGridSchema");
                return StatusCode(500);
            }
        }

        #endregion Settings Management

        #region User Sites

        [Route("GetUserSites")]
        public IActionResult GetUserSites(string userName)
        {
            try
            {
                return Ok(_dataService.GetUserSites(userName));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        [Route("UpdateUserSite")]
        public IActionResult UpdateUserSite(UserSites userSite)
        {
            try
            {
                _dataService.UpdateUserSites(userSite);
                return Ok("Site Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }

        #endregion

        #region Tokens
        [Route("GetTokens")]
        public IActionResult GetTokens()
        {
            try
            {
                var userId = _dataService.GetUserId(User?.Identity?.Name);
                return Ok(_dataService.GetTokens(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }
        [Route("CreateToken")]
        public IActionResult GenerateToken()
        {
            try
            {
                var userId = _dataService.GetUserId(User?.Identity?.Name);
                return Ok(_dataService.GenerateToken(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"GetUserSites:{User?.Identity?.Name}");
                return StatusCode(500);
            }
        }
     
        #endregion
    }
}