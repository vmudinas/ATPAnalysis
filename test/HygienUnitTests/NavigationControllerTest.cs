using Hygiena.Controllers;
using Infrastructure.DataContext;
using Infrastructure.Services;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Xunit;

namespace HygienUnitTests
{
    public class NavigationControllerTest
    {
       // private static readonly INodeServices  _nodeServices;
        public static readonly IEmailSender _emailSender;
        public static readonly IConfiguration _config;
        private static readonly ILoggerFactory _iLoggerFactory = new LoggerFactory();
        public static readonly SignInManager<ApplicationUser> SignInManager;
        public static readonly UserManager<ApplicationUser> UserManager;

    

        [Theory]
        [InlineData("HomeView", "~/Views/PartialViews/HomeView.cshtml")]
        [InlineData("ResultsView", "~/Views/PartialViews/ResultView.cshtml")]
        [InlineData("LocationsView", "~/Views/PartialViews/LocationsView.cshtml")]
        [InlineData("PlansView", "~/Views/PartialViews/PlansView.cshtml")]
        [InlineData("AdminView", "~/Views/PartialViews/AdminView.cshtml")]
        private void TestPartialViews(string partialViewName, string partialViewPath)
        {
            Assert.NotNull(partialViewName);
            Assert.NotNull(partialViewPath);

            Assert.Equal(partialViewPath, ReturnPartialViewName(partialViewName));
        }

        private static string ReturnPartialViewName(string viewName)
        {
      

            var service = new DataService(new DatabaseContext(), null, _emailSender, _config, SignInManager, UserManager);
            var controller = new NavigationController(service, null, _iLoggerFactory, SignInManager, UserManager);


            PartialViewResult result;
            //   const int count = 7;
            switch (viewName)
            {
                case "HomeView":
                    result = controller.HomeView() as PartialViewResult;
                    break;
                case "ResultsView":
                    result = controller.ResultsView() as PartialViewResult;
                    break;
                case "LocationsView":
                    result = controller.LocationsView() as PartialViewResult;
                    break;
                case "PlansView":
                    result = controller.PlansView() as PartialViewResult;
                    break;
                case "AdminView":
                    result = controller.AdminView() as PartialViewResult;
                    break;
                default:
                    return null;
            }
            return result?.ViewName;
        }
    }
}