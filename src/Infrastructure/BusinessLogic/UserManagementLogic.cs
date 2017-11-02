using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.BusinessLogic
{
    public class UserManagementLogic
    {
        public Account CreatedNewAccount(string Address, string City, string CountryCode,
            string DistrictState, string Email, string Name, string Phone, string PostalCode)
        {
            return new Account
            {
                AccountId = Guid.NewGuid(),
                Address = Address,
                City = City,
                CountryCode = CountryCode,
                CreatedDate = DateTime.Now,
                DistrictState = DistrictState,
                Email = Email,
                IsActive = true,
                Name = Name,
                Phone = Phone,
                PostalCode = PostalCode
            };
        }

        public Site CreateDefaultSite(Guid AccountId, string SiteName)
        {
            return new Site
            {
                AccountId = AccountId,
                Name = SiteName,
                SiteId = Guid.NewGuid()
            };
        }

        public SiteUser CreateUserSite(Guid SiteId, string UserId)
        {
            return new SiteUser
            {
                SiteUserId = Guid.NewGuid(),
                SiteId = SiteId,
                UserId = new Guid(UserId)
            };
        }

        public UserAccount CreateUserAccount(Guid AccountId, string UserId)
        {
            return new UserAccount
            {
                AccountId = AccountId,
                UserId = new Guid(UserId),
                UserAccountId = Guid.NewGuid()
            };
        }

        public UserFunctionRole CreateNewRole(Guid AccountId, string RoleDescription, string Role)
        {
            return new UserFunctionRole
            {
                AccountId = AccountId,
                RoleDescription = RoleDescription,
                RoleId = Guid.NewGuid(),
                Role = Role
            };
        }

        public List<UserRolePermission> AdUserRolePermission(List<IdentityRole> roleList, UserFunctionRole useRole)
        {
            var rolePermissionList = new List<UserRolePermission>();
            foreach (var role in roleList)
            {
                var rolePermission = new UserRolePermission
                {
                    Permission = role,
                    Role = useRole,
                    RoleUserPermissionId = Guid.NewGuid()
                };

                rolePermissionList.Add(rolePermission);
            }

            return rolePermissionList;
        }
    }
}