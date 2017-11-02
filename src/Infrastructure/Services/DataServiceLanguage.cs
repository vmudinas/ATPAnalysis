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
        #region Language

        public Dictionary<string, Translation> GetCaptionTranslation(string language, string userName)
        {
            var langDefinition = from b in _repo.Get<LanguageResource>()
                select b;


            var allTranslations = (from t in _repo.Get<LanguageValue>(
                    x => x.Language.LanguageName.Equals(language))
                select t).ToList();

            var translation = new Dictionary<string, Translation>();


            foreach (var value in langDefinition.ToList())
            {
                var translationValues = new Translation
                {
                    Tooltip =
                        allTranslations.Where(x => x.LanguageResource.ResourceValueId == value.ResourceValueId)
                            .Select(x => x.ToolTip).FirstOrDefault()
                        ?? "!Define!",
                    Caption =
                        allTranslations.Where(x => x.LanguageResource.ResourceValueId == value.ResourceValueId)
                            .Select(x => x.Caption).FirstOrDefault()
                        ?? "!Define!",
                    LanguageName = language,
                    LogicNameId = value.ResourceValueId
                };

                translation.Add(value.LogicName, translationValues);
            }


            if (string.IsNullOrWhiteSpace(userName)) return translation;

            try
            {
                var accountId = GetDefaultAccountId(userName);

                var userTranslations = _repo.Get<LanguageUserValue>().Include(x => x.Account).Where(
                    x => x.Language.LanguageName == language && x.Account.AccountId == accountId);

                if (userTranslations == null) return translation;


                foreach (var value in userTranslations.ToList())
                {
                    var existingValue =
                        translation.FirstOrDefault(x => x.Value.LogicNameId == value.LanguageResource.ResourceValueId)
                            .Value;
                    existingValue.Caption =
                        userTranslations.Where(
                                x => x.LanguageResource.ResourceValueId == value.LanguageResource.ResourceValueId)
                            .Select(x => x.Caption).FirstOrDefault();

                    existingValue.Tooltip =
                        userTranslations.Where(
                                x => x.LanguageResource.ResourceValueId == value.LanguageResource.ResourceValueId)
                            .Select(x => x.ToolTip).FirstOrDefault();
                }

                return translation;
            }
            catch (Exception ex)
            {
                var errorTobeLogged = ex;
                return translation;
            }
        }

        public IEnumerable<LanguageResource> GetLogicalName()
        {
            return from b in _repo.Get<LanguageResource>()
                select b;
        }

        public void AddLogicalName(string logicName)
        {
            _repo.Add(new LanguageResource {LogicName = logicName});
            _repo.Save();
        }

        public void DeleteLogicalName(string logicName)
        {
            var itemToRemoveLogicValue = _repo.Get<LanguageResource>().FirstOrDefault(x => x.LogicName == logicName);
            var itemToRemoveUserLanguage =
                _repo.Get<LanguageValue>(x => x.LanguageResource.LogicName == itemToRemoveLogicValue.LogicName);
            var itemToRemoveLanguageResources =
                _repo.Get<LanguageUserValue>(x => x.LanguageResource.LogicName == itemToRemoveLogicValue.LogicName);

            if (itemToRemoveLogicValue == null) return;


            _repo.RemoveRange(itemToRemoveUserLanguage.ToList());
            _repo.RemoveRange(itemToRemoveLanguageResources.ToList());
            _repo.RemoveSave(itemToRemoveLogicValue);
        }

        public void UpdateTranslation(string language, IEnumerable<Translation> languageData)
        {
            var lang = _repo.Get<Language>(x => x.LanguageName == language).FirstOrDefault();
            var getLanguageValues = _repo.Get<LanguageValue>(x => x.Language.LanguageName == language);

            foreach (var translation in languageData)
            {
                var languageValue = new LanguageValue
                {
                    ToolTip = translation.Tooltip,
                    Caption = translation.Caption,
                    Language = lang,
                    LanguageResource =
                        _repo.Get<LanguageResource>(x => x.LogicName == translation.Name).FirstOrDefault()
                };

                if (!getLanguageValues.Any(x => x.LanguageResource.LogicName == translation.Name))
                {
                    _repo.Add(languageValue);
                }
                else
                {
                    var existingValues =
                        getLanguageValues.FirstOrDefault(x => x.LanguageResource.LogicName == translation.Name);
                    existingValues.Caption = languageValue.Caption;
                    existingValues.ToolTip = languageValue.ToolTip;

                    _repo.Update(existingValues);
                }
            }
            _repo.Save();
        }

        public string GetUserLanguage(string userName)
        {
            var lang =
                _repo.Get<UserSetting>(x => x.User.UserName == userName)
                    .Select(x => x.Language.LanguageName)
                    .FirstOrDefault();
            return lang ?? "English";
        }

        public void UpdateUserLanguage(string userName, string languageName)
        {
            var lang = _repo.Get<UserSetting>(x => x.User.UserName == userName);
            var languageNameDb = _repo.Get<Language>(x => x.LanguageName == languageName).FirstOrDefault();
            if (lang.Any())
            {
                foreach (var value in lang)
                    value.Language = languageNameDb;
            }
            else
            {
                var newUserSetting = new UserSetting
                {
                    UserSettingId = Guid.NewGuid(),
                    User = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault(),
                    Language = _repo.Get<Language>(x => x.LanguageName == languageName).FirstOrDefault()
                };

                _repo.Add(newUserSetting);
            }
            _repo.Save();
        }

        #endregion
    }
}