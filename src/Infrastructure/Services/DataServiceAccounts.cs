using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.BusinessLogic;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region Account Registration

        public void CreateNewAccount(RegisterViewModel registerModel, ApplicationUser user)
        {
            var userManagement = new UserManagementLogic();
            // Create New Account 
            var newAccount = userManagement.CreatedNewAccount(registerModel.Address, registerModel.City,
                registerModel.CountryCode, registerModel.DistrictState, registerModel.Email,
                registerModel.Name, registerModel.Phone, registerModel.PostalCode);

            // Create New Site
            var defaultSite = userManagement.CreateDefaultSite(newAccount.AccountId, registerModel.Site);

            // Create new Role
            var userRole = userManagement.CreateNewRole(newAccount.AccountId, "Administrator Role", "Administrator");

            var updateUser = _repo.Get<ApplicationUser>(x => x.Id == user.Id).FirstOrDefault();
            updateUser.UserRole = userRole;

            var adminRoleList = _repo.Get<IdentityRole>(x => !x.Name.Contains("hygiena")).ToList();

            _repo.AddRange(userManagement.AdUserRolePermission(adminRoleList, userRole));
            _repo.Add(newAccount);
            _repo.Add(defaultSite);
            _repo.Add(userManagement.CreateUserSite(defaultSite.SiteId, user.Id));
            _repo.Add(userManagement.CreateUserAccount(newAccount.AccountId, user.Id));
            _repo.Add(userRole);
            _repo.Update(updateUser);

            _repo.Save();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return predicate == null ? _repo.Get<T>().AsQueryable() : _repo.Get(predicate).AsQueryable();
        }

        #endregion
    }
}