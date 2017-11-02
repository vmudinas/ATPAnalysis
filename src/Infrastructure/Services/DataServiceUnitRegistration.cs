using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;
using Infrastructure.UserModel.Models;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region Unit Registration 

        public RegisterUnitConfirmation RegisterUnit(RegisterUnit regUnit, string userName, string password,
            string newUserId)

        {
            var accountId = GetDefaultAccountId(regUnit.UserName);

            var registerUnitConfirmation = new RegisterUnitConfirmation();

            if (
                !_repo.Get<RegisterUnitToken>(
                    x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName).Any())
            {
                registerUnitConfirmation.Error = "1";
                return registerUnitConfirmation;
            }

       
            if (_repo.Get<Unit>(x => x.UnitNo == regUnit.UnitSerialNo && x.AccountId == accountId).Any())
            {
                registerUnitConfirmation.Error = "2";
                return registerUnitConfirmation;
            }

            if (_repo.Get<RegisterUnitToken>(x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName)?.Select(x=>x.UserId)?.FirstOrDefault() != null)
            {
                registerUnitConfirmation.Error = "2";
                return registerUnitConfirmation;
            }

            registerUnitConfirmation.SiteId =
                    _repo.Get<RegisterUnitToken>(
                            x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName)
                        .FirstOrDefault()
                        .SiteId;

                registerUnitConfirmation.AccountId =
                    _repo.Get<RegisterUnitToken>(
                            x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName)
                        .FirstOrDefault()
                        .AccountId;

                AddUserAccount(accountId, regUnit.UserName, newUserId);
                registerUnitConfirmation.Password = password;
                registerUnitConfirmation.UnitUserName = userName;

                var unitId =
                    _repo.Get<RegisterUnitToken>(
                            x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName)
                        ?.FirstOrDefault()
                        ?.UnitId;

            if (!regUnit.IsCloudSync)
            {
                var updateUnit = _repo.Get<Unit>(x => x.Id == unitId).FirstOrDefault();
                updateUnit.UnitNo = regUnit.UnitSerialNo;
                registerUnitConfirmation.UnitName = updateUnit?.UnitName ?? "";
                _repo.Update(updateUnit);
            }
           

                var updateTokenUnit =
                    _repo.Get<RegisterUnitToken>(
                            x => x.Token.ToString() == regUnit.Token && x.CreatorUserName == regUnit.UserName)
                        ?.FirstOrDefault();

            if (updateTokenUnit != null)
            {
                updateTokenUnit.UnitId = unitId;
                updateTokenUnit.UserId = userName;
                updateTokenUnit.IsCloudSync = regUnit.IsCloudSync;
                _repo.Update(updateTokenUnit);
            }

            _repo.Save();

                return registerUnitConfirmation;          

        }

        #endregion

        public int AddUnitToken(string tokenName, string siteId, string userName, string name, bool isCloudSync)
        {
            var accountId = GetDefaultAccountId(userName);

            if (isCloudSync)
            {
                AddNewCloudSync(accountId, siteId, tokenName, userName, name);
            }
            else
            {
                AddNewUnit(accountId, siteId, tokenName, userName, name);
            }           

            return _repo.Save();
        }
        private void AddNewUnit(Guid accountId, string siteId, string tokenName, string userName, string name)
        {
            var unitId = Guid.NewGuid();

            var newToken = new RegisterUnitToken
            {
                TokenId = Guid.NewGuid(),
                AccountId = accountId,
                SiteId = new Guid(siteId),
                Token = tokenName,
                CreatorUserName = userName,
                UnitId = unitId,
                Name = name
            };

            var newUnit = new Unit
            {
                Id = unitId,
                AccountId = accountId,
                SiteId = new Guid(siteId),
                UnitName = name ?? ""
            };

            _repo.Add(newToken);
            _repo.Add(newUnit);
        }
        private void AddNewCloudSync(Guid accountId, string siteId, string tokenName, string userName, string name)
        {
          
            var newToken = new RegisterUnitToken
            {
                TokenId = Guid.NewGuid(),
                AccountId = accountId,
                SiteId = new Guid(siteId),
                Token = tokenName,
                CreatorUserName = userName,
                IsCloudSync = true,
                Name = name
            };

            _repo.Add(newToken);
        }

        public int RemoveUnitToken(string tokenName, string userName)
        {
            var tokenToRemove =
                _repo.Get<RegisterUnitToken>(x => x.Token == tokenName && x.CreatorUserName == userName)
                    ?.FirstOrDefault();
            if (tokenToRemove != null)
            {
                var unitId = tokenToRemove.UnitId;
                _repo.Remove(tokenToRemove);

                if (unitId != null)
                {
                    var unit = _repo.Get<Unit>(x => x.Id == unitId).FirstOrDefault();

                    if (unit != null)
                        _repo.Remove(unit);
                }
            }

            if (tokenToRemove != null && tokenToRemove.UserId == null) return _repo.Save();
            {
                var userAccounts =
                    _repo.Get<UserAccount>(x => x.UserId.ToString() == tokenToRemove.UserId).FirstOrDefault();
                if (userAccounts != null)
                    _repo.Remove(userAccounts);

                var userSettings = _repo.Get<UserSetting>(x => x.User.Id == tokenToRemove.UserId).FirstOrDefault();
                if (userSettings != null)
                    _repo.Remove(userSettings);

                var user = _repo.Get<ApplicationUser>(x => x.Id == tokenToRemove.UserId).FirstOrDefault();
                if (user != null)
                    _repo.Remove(user);
            }
            return _repo.Save();
        }

        public IEnumerable<UnitToken> GetUnitTokens(string userName)
        {
            var tokens = new List<UnitToken>();
            var userid = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id ?? "";
            var accountId = _repo.Get<UserAccount>(x => x.UserId.ToString() == userid).FirstOrDefault().AccountId;
            var sites = _repo.Get<Site>(x => x.AccountId == accountId).ToList();
            var units = _repo.Get<Unit>(x => x.AccountId == accountId).ToList();

            var result = _repo.Get<RegisterUnitToken>(x => x.AccountId == accountId);

            foreach (var value in result)
            {
                var token = new UnitToken
                {
                    Token = value.Token ?? "",
                    SiteName = sites.FirstOrDefault(x => x.SiteId == value.SiteId)?.Name ?? "",
                    Creator = value.CreatorUserName ?? "",
                    Name = value.Name ?? "",
                    UserId =  value.UserId,
                    UnitNo = units?.FirstOrDefault(x => x.Id == value.UnitId)?.UnitNo ?? 0,
                    IsCloudSync = value.IsCloudSync
                };
                tokens.Add(token);
            }

            return tokens;
        }
    }
}