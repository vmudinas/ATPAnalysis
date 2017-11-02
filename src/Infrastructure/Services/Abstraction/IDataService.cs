using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.UnitEntities;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Abstraction
{
    public interface IDataService
    {
        #region Locations

        IEnumerable<Location> GetLocations(string userName);

        #endregion

        #region Plans

        IEnumerable<Plan> GetPlans(string userName);

        bool AddPlansFromUnit(PlanItem unitPlanItem);
        int GetPlanCountFromPlanIdList(List<Guid> planIds);
        Guid GetPlanIdByIdAndModBy(Guid planId, string modifiedBy);
        List<UnitPlan> GetPlanPayload(Guid acctId, int unitNo);
        void DeletePlanList(List<Guid> planIds);

        #endregion Plans

        #region Reports Generation

        IEnumerable<ReportPieChart> GetReports(DateTime toDate, DateTime fromDate, string userName, string reportType);

        #endregion

        #region Results

        IEnumerable<MongoResult> GetResultsByUnit(int unitNo);
        IEnumerable<ClientUnitResult> AddTestResults(ResultsItem unitResultsItem);
        IEnumerable<MongoResult> GetAllResults(Account account);
        IEnumerable<ClientResult> GetResults(DateTime fromUtc, DateTime toUtc, ClientAccount account);
        object GetPagedResults(DateTime fromUtc, DateTime toUtc, DataSourceLoadOptions options, ClientAccount account);
        int GetPagedResultsCount(DateTime fromUtc, DateTime toUtc, DataSourceLoadOptions options, ClientAccount account);

        void UpdateResult(ClientResult cr);

        #endregion Results

        #region Home Screen

        IEnumerable<MongoResult> GetAllHomeData(DateTime fromUtc, DateTime toUtc, ClientAccount account);
      
        #endregion

        #region Language

        Dictionary<string, Translation> GetCaptionTranslation(string language, string userName);
        IEnumerable<LanguageResource> GetLogicalName();
        void AddLogicalName(string logicName);
        void DeleteLogicalName(string logicName);

        void UpdateTranslation(string language, IEnumerable<Translation> languageData);
        string GetUserLanguage(string userName);
        void UpdateUserLanguage(string userName, string languageName);

        #endregion

        #region user language

        IEnumerable<UserLanguage> GetUserDefinedLanguage(string userName, string language);

        IEnumerable<LanguageValue> GetWordsByLanguage(string language);

        void RemoveUserLanguage(string userName, string language, string logicName);

        void UpdateUserLanguage(string userName, string language, string caption, string userCaption, string userToolTip);

        #endregion

        #region User

        bool IsUserUnitOperator(string userName);
        ClientAccount GetCurrentUserAccount(string userName);

        Guid GetDefaultAccountId(string userName);
        Site GetSiteByName(string siteName);
        Site GetSiteById(string siteId);
        IEnumerable<Site> GetAllSites();
        IEnumerable<ClientSites> GetAllSitesForAccount(string userName);
        IEnumerable<UserManagement> GetAllCurrentAccountUsers(string userName);
        ClientUserSetting GetUserSettings(string userName);

        void UpdateUserSettings(string userName, string whichSection, ClientUserSetting cus, DateTime fromUtc,
            DateTime toUtc);

        int AddUnitToken(string tokenName, string siteId, string userName, string unitName, bool isCloudSync);
        int RemoveUnitToken(string tokenName, string userName);
        IEnumerable<UnitToken> GetUnitTokens(string userName);
        RegisterUnitConfirmation RegisterUnit(RegisterUnit regUnit, string userName, string password, string newUserId);

        IEnumerable<UserFunctionRole> GetAllCurrentAccountRoles(string userName);

        IEnumerable<RolePermission> GetRolePermissions(string role);
        IEnumerable<RolePermission> GetAllRolePermissions();

        Dictionary<string, bool> GetUserRolePermissions(string userName);

        void AddUser(ClientAccount account, UserManagement user);
        void RemoveUser(string userName);

        void UpdateUser(ClientAccount account, UserManagement user, string oldUserName);

        void AddRole(Guid roleId, string role, string roleDescription, ClientAccount accout);
        void RemoveRole(string roleId);
        void UpdateRole(string roleId, string role, string roleDescription);

        void AddPermission(string role, string permission, ClientAccount account);

        void RemovePermission(string role, string permission, ClientAccount account);

        void UpdateResultGridSchema(string userName, string resultGridSchemaJson);
        string GetResultGridSchema(string userName);

        #endregion

        #region User Sites

        IEnumerable<UserSites> GetUserSites(string userName);

        void UpdateUserSites(UserSites userSite);

        #endregion

        #region Account Registration

        void CreateNewAccount(RegisterViewModel registerModel, ApplicationUser user);


        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        #endregion

        #region Report Scheduling

        IEnumerable<ClientReportSchedule> GetReportSchedules(string userName);
        void UpdateReportSchedule(ClientReportSchedule crs, string userName);
        void AddReportSchedule(ClientReportSchedule crs, string userName);
        void DeleteReportSchedule(string accountId, string scheduleId);
        void SendScheduledReports();

        #endregion Report Scheduling

        #region Node Services

         string RunNodeReport(DateTime fromDate, DateTime toUtc, string userName,
            string reportType, string reportTitle, List<string> emailList);

        void GenerateGeneric<T>(IEnumerable<T> entity) where T : class;

        void GenerateNoSqlFromSql();
        #endregion


        #region CloudSync 

        CloudSyncReturn AddResultCloudSync(ClientCloudSyncResult cloudSyncData);

        CloudSyncReturn AddResultCloudSyncMongo(ClientCloudSyncResult cloudSyncData);

        #endregion
        #region Generate Token
        List<Token> GetTokens(Guid userId);
        int GenerateToken(Guid userId);
        bool ExpireToken(string token, string unitSerial);
        #endregion

        void MigrationLanguages();

        Guid GetUserId(string userName);
    }
}