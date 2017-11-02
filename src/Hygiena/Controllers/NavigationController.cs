using System.Threading.Tasks;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Models.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using static Hygiena.Controllers.ManageController;
using System;

namespace Hygiena.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;
        private readonly INodeServices _nodeServices;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public NavigationController(IDataService dataService, INodeServices nodeServices, ILoggerFactory loggerFactory,
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _dataService = dataService;
            _nodeServices = nodeServices;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("Reports")]
        public IActionResult ReportView(string userName)
        {
           UserNameLogin(userName).GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(2000);

            return RedirectToAction(nameof(MainView));
        }

    
        private async   Task UserNameLogin(string userName)
        {
            var user =   await _userManager.FindByNameAsync(userName);

           if (user != null)
            {
                await _signInManager.SignInAsync(user, true);
          
           }

        }


        [AllowAnonymous]
        public IActionResult MainView()
        {
            return View();
        }

        [Authorize(Roles = "homeViewEnabled")]
        public IActionResult HomeView()
        {
            return PartialView("~/Views/PartialViews/HomeView.cshtml");
        }

        [Authorize(Roles = "resultViewEnabled")]
        public IActionResult ResultsView()
        {
            return PartialView("~/Views/PartialViews/ResultView.cshtml");
        }

        [Authorize(Roles = "reportsViewEnabled")]
        public IActionResult ReportsView()
        {
            return PartialView("~/Views/PartialViews/ReportsView.cshtml");
        }

        
            

        [Authorize(Roles = "unitViewEnabled")]
        public IActionResult UnitsView()
        {
            return PartialView("~/Views/PartialViews/UnitsView.cshtml");
        }


        [Authorize(Roles = "adminUnitViewEnabled")]
        public IActionResult AdminUnitView()
        {
            return PartialView("~/Views/PartialViews/AdminUnitView.cshtml");
        }

        [Authorize(Roles = "locationsViewEnabled")]
        public IActionResult LocationsView()
        {
            return PartialView("~/Views/PartialViews/LocationsView.cshtml");
        }

        [Authorize(Roles = "plansViewEnabled")]
        public IActionResult PlansView()
        {
            return PartialView("~/Views/PartialViews/PlansView.cshtml");
        }

        [Authorize(Roles = "adminViewEnabled")]
        public IActionResult AdminView()
        {
            return PartialView("~/Views/PartialViews/AdminView.cshtml");
        }

        [Authorize(Roles = "languageViewEnabled")]
        public IActionResult LanguageView()
        {
            return PartialView("~/Views/PartialViews/LanguageView.cshtml");
        }

        [Authorize(Roles = "adminUserLanguageViewEnabled")]
        public IActionResult AdminUserLanguageView()
        {
            return PartialView("~/Views/PartialViews/AdminUserLanguageView.cshtml");
        }

        [Authorize(Roles = "adminUserManagementViewEnabled")]
        public IActionResult AdminUserManagementView()
        {
            return PartialView("~/Views/PartialViews/AdminUserManagementView.cshtml");
        }

        [Authorize(Roles = "adminRoleManagementViewEnabled")]
        public IActionResult RoleManagementView()
        {
            return PartialView("~/Views/PartialViews/RoleManagementView.cshtml");
        }

        [Authorize(Roles = "canScheduleEmail")]
        public IActionResult ScheduleEmailedReportsView()
        {
            return PartialView("~/Views/PartialViews/ScheduleEmailedReportsView.cshtml");
        }

     //   [Authorize(Roles = "hygienaSupport")]
        public IActionResult TokenGeneratorView()
        {
            return PartialView("~/Views/PartialViews/TokenGeneratorView.cshtml");
        }


        [HttpGet]
        [Route("NavigateLogOff")]
        public async Task<IActionResult> NavigateLogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");

            return RedirectToAction(nameof(MainView));
        }


        [HttpGet]
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return PartialView("~/Views/Manage/ChangePassword.cshtml");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(MainView), new {Message = ManageMessageId.ChangePasswordSuccess});
                }
                AddErrors(result);
                return PartialView(model);
            }
            return RedirectToAction(nameof(MainView), new {Message = ManageMessageId.Error});
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}