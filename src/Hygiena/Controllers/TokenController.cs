using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hygiena.Controllers
{
    [AllowAnonymous]
    [Route("api")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IDataService _dataService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmsSender _smsSender;
        private readonly TokenAuthOptions _tokenOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory, IConfiguration configuration,
            TokenAuthOptions tokenOptions,
            IDataService dataService)
        {
            _tokenOptions = tokenOptions;
            //this.bearerOptions = options.Value;
            //this.signingCredentials = signingCredentials;
            _dataService = dataService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _config = configuration;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        /// <summary>
        ///     Check if currently authenticated. Will throw an exception of some sort which should be caught by a general
        ///     exception handler and returned to the user as a 401, if not authenticated. Will return a fresh token if
        ///     the user is authenticated, which will reset the expiry.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Bearer")]
        public dynamic Get()
        {


            var authenticated = false;
            string user = null;
            var entityId = -1;
            string token = null;
            var tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser == null)
                return
                    new
                    {
                        authenticated,
                        user,
                        entityId,
                        token,
                        tokenExpires
                    };
            authenticated = currentUser.Identity.IsAuthenticated;
            if (authenticated)
            {
                user = currentUser.Identity.Name;
                foreach (var c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                tokenExpires = DateTime.UtcNow.AddHours(4);
                token = GetToken(currentUser.Identity.Name, tokenExpires);
            }
            return
                new
                {
                    authenticated,
                    user,
                    entityId,
                    token,
                    tokenExpires
                };
        }

        /// <summary>
        ///     Request a new token for a given username/password pair.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Post([FromBody] AuthRequest authRequest)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(authRequest.Username);
                if (user == null)
                    return Json(
                        new
                        {
                            authenticated = false,
                            errorCode = 8
                        });
                if (!await _userManager.CheckPasswordAsync(user, authRequest.Password))
                    return Json(
                        new
                        {
                            authenticated = false,
                            errorCode = 9
                        });

                // if (result == PasswordVerificationResult.Success) { return user; }


                var result = await _signInManager.PasswordSignInAsync(authRequest.Username, authRequest.Password, true,
                    false);


                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.", authRequest.Username);
                    if (!_dataService.IsUserUnitOperator(authRequest.Username))
                        return new JsonResult(new {authenticated = false, message = "User not Unit Operator"});
                    DateTime? expires = DateTime.UtcNow.AddHours(4);
                    var token = GetToken(authRequest.Username, expires);
                    return
                        new JsonResult(new {authenticated = true, entityId = 1, token, tokenExpires = expires});
                }
                if (result.RequiresTwoFactor)
                    return Json(
                        new
                        {
                            errorCode = 5
                        });
                if (result.IsLockedOut)
                    return Json(
                        new
                        {
                            errorCode = 5
                        });

                if (result.IsNotAllowed)
                    return Json(
                        new
                        {
                            errorCode = 6
                        });
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"Login failed:{authRequest.Username}");
                return
                    Json(
                        new
                        {
                            errorCode = 10
                        });
            }

            return Json(
                new
                {
                    errorCode = 7
                });
        }

        //[HttpPost]
        //[Route("reg")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Reh([FromBody] RegisterUnit unitRegister)
        //{
        //    //[FromBody] RegisterUnit unitRegister
        //    return Ok(unitRegister);
        //}

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUnit([FromBody] RegisterUnit unitRegister)
        {
            var sync = unitRegister.IsCloudSync ? "cloudSync":"unit";
            var newUserId = Guid.NewGuid().ToString();
            var user = new ApplicationUser
            {
                UserName = newUserId,
                Id = newUserId,
                AccessFailedCount = 0,
                EmailConfirmed = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                Email = $"{sync}@hygiena.com",
                NormalizedEmail = $"{sync}@HYGIENA.COM"
            };
            try
            {
                var adminRoles = _dataService.Get<IdentityRole>(x => x.Name.Contains("unitOperatorEnabled"));

                foreach (var adminRole in adminRoles)
                {
                    var role = new IdentityUserRole<string>
                    {
                        RoleId = adminRole.Id,
                        UserId = user.Id
                    };
                    user.Roles.Add(role);
                }
                var password = GeneratePassword();
                var result = await _userManager.CreateAsync(user, password);
                // Create New Account with Default Site
                if (result.Succeeded)
                {
                    var registrationResult = _dataService.RegisterUnit(unitRegister, user.UserName, password, newUserId);
                    if (!string.IsNullOrWhiteSpace(registrationResult.Error))
                    {
                        await _userManager.DeleteAsync(user);

                    }
                    return Json(registrationResult);
                }
                await _userManager.DeleteAsync(user);
                var registerUnitConfirmation = new RegisterUnitConfirmation
                {
                    Error = "3"
                };
                return Ok(registerUnitConfirmation);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"Register Failed. Serial Number:{unitRegister?.UnitSerialNo}");
                await _userManager.DeleteAsync(user);

                var registerUnitConfirmation = new RegisterUnitConfirmation
                {
                    Error = "4"
                };
                return Ok(registerUnitConfirmation);
            }
        }

        private string GeneratePassword()
        {
            // Generates random password with Upper and Lower case letters
            return Guid.NewGuid() + "Aa!";
        }

        [HttpGet]
        [Authorize(Roles = "adminUnitViewEnabled")]
        [Route("GenerateToken")]
        public IActionResult GenerateToken(string siteId, string tokenName, string name, string isCloudSync)
        {
            try
            {
                var account = _dataService.GetCurrentUserAccount(User.Identity.Name);
                return Ok(_dataService.AddUnitToken(tokenName, siteId, User.Identity.Name, name, Convert.ToBoolean(isCloudSync)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message, ex.Message, User.Identity.Name);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "adminUnitViewEnabled")]
        [Route("GetUnitTokens")]
        public IActionResult GetUnitTokens()
        {
            try
            {
                return Ok(_dataService.GetUnitTokens(User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, $"GetUnitTokens{User?.Identity?.Name ?? ""}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "adminUnitViewEnabled")]
        [Route("RemoveToken")]
        public IActionResult RemoveToken(string tokenName, string userName)
        {
            try
            {
                return Ok(_dataService.RemoveUnitToken(tokenName, userName));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex, "RemoveToken");
                return StatusCode(500);
            }
        }


        private string GetToken(string user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.
            var identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth"),
                new[] {new Claim("EntityID", "1", ClaimValueTypes.Integer)});

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                SigningCredentials = _tokenOptions.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }

        public class AuthRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}