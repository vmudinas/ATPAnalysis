using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region User Language

        public IEnumerable<UserLanguage> GetUserDefinedLanguage(string userName, string language)
        {
            var result =
                _repo.Get<LanguageUserValue>().Include(x => x.Account)
                    .Include(x => x.Language)
                    .Include(x => x.LanguageResource)
                    .Where(
                        x =>
                            x.Account.AccountId == GetDefaultAccountId(userName) &&
                            x.Language.LanguageName == language).ToList();
            var languageValues =
                _repo.Get<LanguageValue>().Include(x => x.LanguageResource)
                    .Include(x => x.Language)
                    .Where(x => x.Language.LanguageName == language);

            return result.Select(value => new UserLanguage
            {
                UserCaption = value.Caption,
                UserToolTip = value.ToolTip,
                Caption =
                    languageValues.FirstOrDefault(x => x.LanguageResource.LogicName == value.LanguageResource.LogicName)
                        .Caption,
                ToolTip =
                    languageValues.FirstOrDefault(x => x.LanguageResource.LogicName == value.LanguageResource.LogicName)
                        .ToolTip,
                Language = language,
                LogicName = value.LanguageResource.LogicName
            }).ToList();
        }

        public void RemoveUserLanguage(string userName, string language, string logicName)
        {
            var userid = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id;
            var accountId = _repo.Get<UserAccount>(x => x.UserId.ToString() == userid).FirstOrDefault().AccountId;

            var toBeDeleted =
                _repo.Get<LanguageUserValue>().Include(x => x.Account)
                    .Include(x => x.Language)
                    .Include(x => x.LanguageResource)
                    .Where(
                        x =>
                            x.Account.AccountId == accountId &&
                            x.Language.LanguageName == language && x.LanguageResource.LogicName == logicName).ToList();

            _repo.RemoveRange(toBeDeleted);
            _repo.Save();
        }

        public void UpdateUserLanguage(string userName, string language, string caption, string userCaption,
            string userToolTip)
        {
            var userid = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id ?? "";
            var accountId = _repo.Get<UserAccount>(x => x.UserId.ToString() == userid).FirstOrDefault().AccountId;

            var langResource =
                _repo.Get<LanguageValue>().Include(x => x.LanguageResource)
                    .Include(x => x.Language)
                    .FirstOrDefault(x => x.Caption == caption && x.Language.LanguageName == language)
                    .LanguageResource;
            var valueToUpdate =
                _repo.Get<LanguageUserValue>().Include(x => x.LanguageResource)
                    .Include(x => x.Account)
                    .Include(x => x.Language)
                    .FirstOrDefault(
                        x =>
                            x.Language.LanguageName == language &&
                            x.LanguageResource.ResourceValueId == langResource.ResourceValueId &&
                            x.Account.AccountId == accountId);
            if (valueToUpdate == null)
            {
                //Add New
                var newLanguageValue = new LanguageUserValue
                {
                    LanguageUserId = Guid.NewGuid(),
                    Account = _repo.Get<Account>(x => x.AccountId == accountId).FirstOrDefault(),
                    Caption = userCaption,
                    ToolTip = userToolTip,
                    Language = _repo.Get<Language>(x => x.LanguageName == language).FirstOrDefault(),
                    LanguageResource =
                        _repo.Get<LanguageValue>().Include(x => x.Language)
                            .Include(x => x.LanguageResource)
                            .FirstOrDefault(x => x.Language.LanguageName == language && x.Caption == caption)
                            .LanguageResource
                };

                _repo.Add(newLanguageValue);
            }
            else
            {
                valueToUpdate.Caption = userCaption;
                valueToUpdate.ToolTip = userToolTip;
                _repo.Update(valueToUpdate);
            }

            _repo.Save();
        }

        public IEnumerable<LanguageValue> GetWordsByLanguage(string language)
        {
            return
                _repo.Get<LanguageValue>().Include(x => x.Language)
                    .Include(x => x.LanguageResource)
                    .Where(x => x.Language.LanguageName == language)
                    .ToList();
        }

        #endregion
    }
}