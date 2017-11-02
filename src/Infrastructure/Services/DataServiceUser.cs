using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.UserModel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public partial class DataService
    {
        public IEnumerable<UserFunctionRole> GetAllCurrentAccountRoles(string userName)
        {
            return _repo.Get<UserFunctionRole>(x => x.AccountId == GetDefaultAccountId(userName));
        }

        public IEnumerable<UserManagement> GetAllCurrentAccountUsers(string userName)
        {

            var userList =
                _repo.Get<ApplicationUser>().Include("UserRole").Where(x => x.Email != "unit@hygiena.com" && x.Email != "cloudSync@hygiena.com").ToList();

            var users = _repo.Get<UserAccount>(x => x.AccountId == GetDefaultAccountId(userName)).ToList();
            var sites = _repo.Get<SiteUser>(x => x.UserId == GetUserId(userName)).ToList();
            var clientSites = sites.Select(value => new ClientSites
            {
                Id = value?.SiteId?.ToString() ?? string.Empty, Name = _repo.Get<Site>(x => x.SiteId == value.SiteId).FirstOrDefault()?.Name ?? string.Empty
            }).ToList();


            return (from value in userList
                where users.FirstOrDefault(x => x.UserId == Guid.Parse(value.Id)) != null
                select new UserManagement
                {
                    UserName = value?.UserName ?? string.Empty, Email = value?.Email ?? string.Empty, RoleName = value?.UserRole?.Role ?? string.Empty, Sites = clientSites
                }).ToList();
        }

        public IEnumerable<RolePermission> GetAllRolePermissions()
        {
            var permissions = _repo.Get<IdentityRole>();

            var permissionList = permissions.Select(value => new RolePermission
            {
                PermissionName = value.Name,
                PermissionDescription = value.NormalizedName ?? string.Empty
            }).ToList();

            return permissionList;
        }

        public IEnumerable<Site> GetAllSites()
        {
            return _repo.Get<Site>();
        }

        public IEnumerable<ClientSites> GetAllSitesForAccount(string userName)
        {
            var accountId =
                _repo.Get<UserAccount>(x => x.UserId == GetUserId(userName)).FirstOrDefault().AccountId;
            var siteResults = _repo.Get<Site>(x => x.AccountId == accountId).ToArray();
            return
                siteResults.Select(value => new ClientSites {Id = value.SiteId.ToString(), Name = value.Name}).ToList();
        }


        public ClientAccount GetCurrentUserAccount(string userName)

        {
            var accountId = GetDefaultAccountId(userName);
            var userId = GetUserId(userName);
           var account =   _repo.Get<Account>(x => x.AccountId == accountId).FirstOrDefault();
            return new ClientAccount
            {
                AccountId = account.AccountId,
                UserName = userName,
                Sites = _repo.Get<SiteUser>(x => x.UserId == userId).ToList()
            };

        }

        public Guid GetDefaultAccountId(string userName)

        {
            return _repo.Get<UserAccount>(x => x.UserId == GetUserId(userName)).FirstOrDefault().AccountId;
        }

        public IEnumerable<RolePermission> GetRolePermissions(string role)
        {
            var roleId = _repo.Get<UserFunctionRole>(x => x.Role == role).FirstOrDefault().RoleId;
            var roles =
                _repo.Get<UserRolePermission>().Include(x => x.Role)
                    .Include(x => x.Permission)
                    .Where(x => x.Role.RoleId == roleId)
                    .ToList();

            var currentPermissions = roles.Select(value => new RolePermission
            {
                PermissionName = value.Permission.Name,
                PermissionDescription = value.Permission.NormalizedName,
                PermissionId = value.Permission.Id
            }).ToList();

            var allPermissions = _repo.Get<IdentityRole>();

            var permissionList =
                allPermissions.Where(x => x.Name != "unitOperatorEnabled").Select(value => new RolePermission
                {
                    PermissionName = value.Name,
                    PermissionDescription = value.NormalizedName ?? string.Empty,
                    Status = currentPermissions.Any(x => x.PermissionId == value.Id)
                }).ToList();

            return permissionList;
        }

        public Site GetSiteById(string siteId)
        {
            return _repo.Get<Site>(x => x.SiteId.ToString().Equals(siteId)).FirstOrDefault();
        }


        public Site GetSiteByName(string siteName)
        {
            return _repo.Get<Site>(x => x.Name.Equals(siteName)).FirstOrDefault();
        }

        public Dictionary<string, bool> GetUserRolePermissions(string userName)
        {
            var userRoles = _repo.Get<IdentityUserRole<string>>(x => x.UserId == GetUserIdStr(userName)).ToList();

            var permissions = _repo.Get<IdentityRole>().ToList();

            var permissionList = permissions.ToDictionary(value => value.Name,
                value => userRoles.Any(x => Equals(x.RoleId, value.Id)));


            return permissionList;
        }

        public ClientUserSetting GetUserSettings(string userName)
        {
            var us =
                _repo.Get<UserSetting>().Include(x => x.User).FirstOrDefault(x => x.User.UserName == userName);

            return new ClientUserSetting
            {
                DashDateFrom = us?.DashDateFrom,
                DashDateTo = us?.DashDateTo,
                DashPeriod = us?.DashPeriod,
                DashType = us?.DashType,
                ResultsDateFrom = us?.ResultsDateFrom,
                ResultsDateTo = us?.ResultsDateTo,
                ResultsPeriod = us?.ResultsPeriod,
                ReportScheduleCurrentView = us?.ReportScheduleCurrentView
            };
        }

        public bool IsUserUnitOperator(string userName)
        {
            var unitRole = _repo.Get<IdentityRole>(x => x.Name == "unitOperatorEnabled").FirstOrDefault();

            var roles =
                _repo.Get<IdentityUserRole<string>>(x => x.UserId == GetUserIdStr(userName) && x.RoleId == unitRole.Id);
            return roles.Any();
        }

        public void UpdateUserSettings(string userName, string whichSection, ClientUserSetting cus, DateTime fromUtc,
            DateTime toUtc)
        {
            var us =
                _repo.Get<UserSetting>().Include(x => x.User).FirstOrDefault(x => x.User.UserName == userName);

            if (us == null) return;
            if (!string.IsNullOrWhiteSpace(whichSection))
            {
                whichSection = whichSection.ToLower().Trim();

                // for some reason, dates are not serializing, so passing strings, then converting once arrived at server. To prevent 
                // excess parameters, just using two "date" params, then assigning the specific section date by "whichSection" param
                switch (whichSection)
                {
                    case "dash":
                        us.DashDateFrom = fromUtc;
                        us.DashDateTo = toUtc;
                        us.DashPeriod = cus.DashPeriod;
                        us.DashType = cus.DashType;
                        break;
                    case "results":
                        us.ResultsDateFrom = fromUtc;
                        us.ResultsDateTo = toUtc;
                        us.ResultsPeriod = cus.ResultsPeriod;
                        break;
                    case "reportschedule":
                        us.ReportScheduleCurrentView = cus.ReportScheduleCurrentView;
                        break;
                }
            }

            _repo.UpdateSave(us);
        }

        private string GetUserIdStr(string userName)
        {
            return _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id ?? string.Empty;
        }
        public Guid GetUserId(string userName)
        {
            return Guid.Parse(_repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id) ;
        }

        public IEnumerable<UserAccount> GetAccountList(string userName)

        {
            return _repo.Get<UserAccount>(x => x.UserId == GetUserId(userName));
        }

        #region User Management

        public void AddUser(ClientAccount account, UserManagement user)
        {
            var roles =
                _repo.Get<UserFunctionRole>(x => x.AccountId == account.AccountId && x.Role == user.RoleName)
                    .FirstOrDefault();
            var userRolesPermission =
                _repo.Get<UserRolePermission>().Include(x => x.Permission)
                    .Where(x => x.Role.AccountId == account.AccountId && x.Role.Role == user.RoleName);
            var addUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = user.Email.ToLower(),
                NormalizedEmail = user.Email,
                UserName = user.UserName.ToLower(),
                NormalizedUserName = user.UserName,
                UserRole = roles,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = Guid.NewGuid().ToString()
            };

            foreach (var value in userRolesPermission)
            {
                var identityUser = new IdentityUserRole<string>
                {
                    UserId = addUser.Id,
                    RoleId = value.Permission.Id
                };

                _repo.Add(identityUser);
            }
            _repo.Add(addUser);


            var userAccount = new UserAccount
            {
                UserId = new Guid(addUser.Id),
                AccountId = account.AccountId,
                UserAccountId = Guid.NewGuid()
            };
            _repo.Add(userAccount);
            _repo.Save();
        }

        public void AddUserAccount(Guid accountId, string userName, string newUserId)
        {
            var userAccount = new UserAccount
            {
                UserId = new Guid(newUserId),
                AccountId = accountId,
                UserAccountId = Guid.NewGuid()
            };
            _repo.AddSave(userAccount);
        }

        public void RemoveUser(string userName)
        {
            var user = _repo.Get<ApplicationUser>()
                .Include(x => x.UserRole).Include(x => x.Roles).Include(x => x.Claims).Include(x => x.Logins)
                .FirstOrDefault(x => x.UserName == userName);

            var userInAccount = _repo.Get<UserAccount>(x => x.UserId.ToString() == user.Id).ToList();
            var userSite = _repo.Get<SiteUser>(x => x.UserId.ToString() == user.Id).ToList();
            var usersettings = _repo.Get<UserSetting>(x => x.User == user).ToList();
            var userRoles = _repo.Get<IdentityUserRole<string>>(x => x.UserId == user.Id).ToList();


            _repo.RemoveRange(userInAccount);
            _repo.RemoveRange(userSite);
            _repo.RemoveRange(usersettings);
            _repo.RemoveRange(userRoles);
            _repo.Remove(user);

            _repo.Save();
        }

        public void UpdateUser(ClientAccount account, UserManagement user, string oldUserName)
        {
            var roles =
                _repo.Get<UserFunctionRole>(x => x.AccountId == account.AccountId && x.Role == user.RoleName)
                    .FirstOrDefault();
            var userRolesPermission =
                _repo.Get<UserRolePermission>().Include(x => x.Permission)
                    .Where(x => x.Role.AccountId == account.AccountId && x.Role.Role == user.RoleName);
            var addUser = _repo.Get<ApplicationUser>(x => x.UserName == oldUserName).FirstOrDefault();
            if (roles != null)
                addUser.UserRole = roles;
            if (user?.UserName != null)
            {
                addUser.UserName = user.UserName.ToLower();
                addUser.NormalizedUserName = user.UserName.ToUpper();
            }

            if (user?.Email != null)
            {
                addUser.Email = user.Email;
                addUser.NormalizedEmail = user.Email.ToUpper();
            }

            _repo.Update(addUser);

            var userRoles = _repo.Get<IdentityUserRole<string>>(x => x.UserId == addUser.Id);
            _repo.RemoveRangeSave(userRoles.AsNoTracking().ToList());

            var addUserPermissionList = new List<IdentityUserRole<string>>();
            foreach (var value in userRolesPermission)
            {
                var addUserPermission = new IdentityUserRole<string>
                {
                    RoleId = value.Permission.Id,
                    UserId = addUser.Id
                };

                addUserPermissionList.Add(addUserPermission);
            }

            _repo.AddSaveRange(addUserPermissionList);
        }

        #endregion

        #region User Sites

        public IEnumerable<UserSites> GetUserSites(string userName)
        {
            var allSites = GetAllSitesForAccount(userName);

            var userId = GetUserId(userName);

            return allSites.Select(value => new UserSites
                {
                    Id = value.Id,
                    UserId = userId.ToString(),
                    Site = value.Name,
                    Selected = _repo.Get<SiteUser>(x => x.UserId == userId && x.SiteId.ToString() == value.Id).Any()
                })
                .ToList();
        }

        public void UpdateUserSites(UserSites userSite)
        {
            var updatedUserSite =
                _repo.Get<SiteUser>(x => x.SiteId.ToString() == userSite.Id );

            if (updatedUserSite.Any(x => x.UserId.ToString() == userSite.UserId))
            {
                if (userSite.Selected)
                    _repo.RemoveSave(updatedUserSite.FirstOrDefault());
            }
            else if (!userSite.Selected)
            {
             
                var newSite = new SiteUser
                {  
                    SiteUserId =  Guid.NewGuid(),
                    UserId = Guid.Parse(userSite.UserId),
                    SiteId = Guid.Parse(userSite.Id)

                };

                _repo.AddSave(newSite);
            }
        }

        #endregion

        #region Role Management

        public void AddRole(Guid roleId, string role, string roleDescription, ClientAccount account)
        {
            var userRole = new UserFunctionRole
            {
                AccountId = account.AccountId,
                RoleDescription = roleDescription,
                RoleId = roleId,
                Role = role
            };
            _repo.AddSave(userRole);
        }

        public void RemoveRole(string roleId)
        {
            var user =
                _repo.Get<ApplicationUser>().Include(x => x.UserRole).Where(x => x.UserRole.RoleId.ToString() == roleId);
            foreach (var value in user)
            {
                value.UserRole = null;
                _repo.Update(value);
            }
            var role = _repo.Get<UserFunctionRole>();

            var userRolePermission = _repo.Get<UserRolePermission>(x => x.Role.RoleId.ToString() == roleId);

            // Remove Role - Permission relationship
            _repo.RemoveRange(userRolePermission.ToList());
            _repo.RemoveSave(role.FirstOrDefault(x => x.RoleId.ToString() == roleId));
        }

        public void UpdateRole(string roleId, string role, string roleDescription)
        {
            var roleToUpdate = _repo.Get<UserFunctionRole>(x => x.RoleId.ToString() == roleId).FirstOrDefault();

            if (role != null)
                roleToUpdate.Role = role;
            if (roleDescription != null)
                roleToUpdate.RoleDescription = roleDescription;
            _repo.UpdateSave(roleToUpdate);
        }

        #endregion

        #region Permission Management

        public void AddPermission(string role, string permission, ClientAccount account)
        {
            var user =
                _repo.Get<ApplicationUser>()
                    .Include(x => x.UserRole)
                    .Where(x => x.UserRole.AccountId == account.AccountId);
            var roleList = _repo.Get<IdentityRole>().FirstOrDefault(x => x.Name == permission);
            var userRole = _repo.Get<UserFunctionRole>().FirstOrDefault(x => x.Role == role);
            var roleExist = _repo.Get<IdentityUserRole<string>>().Where(x => x.RoleId == roleList.Id).ToList();

            foreach (var value in user)
            {
                if (value.UserRole.Role != role) continue;


                if (roleExist.Any(x => x.UserId == value.Id)) continue;
                var identityUser = new IdentityUserRole<string>
                {
                    UserId = value.Id,
                    RoleId = roleList.Id
                };
                _repo.Add(identityUser);
            }


            var exist =
                _repo.Get<UserRolePermission>(x => x.Permission == roleList && x.Role == userRole).FirstOrDefault();
            if (exist == null)
            {
                var userRolePermission = new UserRolePermission
                {
                    RoleUserPermissionId = Guid.NewGuid(),
                    Permission = roleList,
                    Role = userRole
                };
                _repo.Add(userRolePermission);
            }
            else
            {
                exist.Permission = roleList;
                exist.Role = userRole;
                _repo.Update(exist);
            }

            _repo.Save();
        }

        public void RemovePermission(string role, string permission, ClientAccount account)
        {
            var permissionRole = _repo.Get<UserRolePermission>().Include(x => x.Permission)
                .FirstOrDefault(x => x.Permission.Name == permission && x.Role.AccountId == account.AccountId);
            _repo.Remove(permissionRole);
            var userRPermission = _repo.Get<IdentityUserRole<string>>(x => x.RoleId == permissionRole.Permission.Id);
            _repo.RemoveRange(userRPermission.ToList());
            _repo.Save();
        }

        #endregion

        #region User Settings

        public void UpdateResultGridSchema(string userName, string resultGridJson)
        {
            var user = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault();
            var userSettings =
                _repo.Get<UserSetting>().Include(x => x.User).FirstOrDefault(x => x.User.UserName == userName);
            if (userSettings == null)
            {
                var newUserSettings = new UserSetting
                {
                    ResultGridSchema = resultGridJson,
                    UserSettingId = Guid.NewGuid(),
                    User = user
                };
                _repo.Add(newUserSettings);
            }
            else
            {
                userSettings.ResultGridSchema = resultGridJson;
                _repo.Update(userSettings);
            }


            _repo.Save();
        }

        public string GetResultGridSchema(string userName)
        {
            var resultGridSchemaJson =
                _repo.Get<UserSetting>()
                    .Include(x => x.User)
                    .Where(x => x.User.UserName == userName)
                    .Select(x => x.ResultGridSchema)
                    .FirstOrDefault();
            return string.IsNullOrWhiteSpace(resultGridSchemaJson) ? string.Empty : resultGridSchemaJson;
        }

        #endregion User Settings

        #region Token Generation
        public List<Token> GetTokens(Guid userId)
        {
            return _repo.Get<Token>().Where(x=>x.Active == true && x.UserId == userId && x.CreatedDate >= DateTime.UtcNow ).ToList();
        }

        public int GenerateToken(Guid userId)
        {
            Random generator = new Random();

            var newToken = new Token
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Tokens = generator.Next(0, 100000).ToString("D5"),
                CreatedDate = DateTime.UtcNow.AddDays(1),
                Active = true
            };

            _repo.AddUpdate(newToken);
           return _repo.Save();
        }

        public bool ExpireToken(string token, string unitSerial)
        {
          var tokenToUpdate = _repo.Get<Token>().Where(x => x.Tokens == token && x.CreatedDate >= DateTime.UtcNow && x.Active == true).FirstOrDefault();

            if (tokenToUpdate != null)
            {              
                    tokenToUpdate.Active = false;
                    tokenToUpdate.UnitSerial = unitSerial;

                    if (_repo.UpdateSave(tokenToUpdate) == 1) return true;               
            }

            return false; 
        }
        #endregion 

    }
}