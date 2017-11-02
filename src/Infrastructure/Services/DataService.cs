using Infrastructure.DataContext;
using Infrastructure.Repository;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        private readonly IRepository _repo;
        private readonly  INodeServices _nodeServices;
        protected readonly IEmailSender EmailSender;
        protected readonly IConfiguration Config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataService(DatabaseContext context, INodeServices nodeServices, IEmailSender emailSender, IConfiguration config, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            
             var client = new MongoClient("mongodb://hygiena:QLFolxkD0V29uu3xKJerMy1nthBgXmfnPHpsvOOiFWAjNMxM5zmvjxREdmnPRPNU7XBP7dRFAq0xenpZrmWiFg==@hygiena.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var mongoDatabase = client.GetDatabase("hygienaMongo");

            _nodeServices = nodeServices;
            _repo = new Repository.Repository(context, mongoDatabase);
            EmailSender = emailSender;
            Config = config;
            _signInManager = signInManager;           
            _userManager = userManager;
        }


     }
}