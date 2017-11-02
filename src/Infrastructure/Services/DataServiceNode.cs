using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading;
using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Microsoft.AspNetCore.NodeServices;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading.Tasks;
using Infrastructure.MongoEntities;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        

        #region Pass Caution Fail

        public string RunNodeReport(DateTime fromDate, DateTime toUtc, string userName,
            string reportType,string reportTitle,List<string> emailList)
        {
           var reportName= $"Report_{reportTitle}_{Guid.NewGuid()}";
        
            var pathToFile = _nodeServices.InvokeExportAsync<string>("./wwwroot/node/reportGenerator", "vitas", $"{Config.GetSection("ReportUrl").Value.TrimEnd('/')}?username={userName}", reportName)
                  .Result;          


             for (var i = 0; i < 99999999; ++i)
             {
                 try
                 {
                    File.OpenRead(pathToFile);
                    {
                        foreach (var email in emailList)
                        {

                            EmailSender.SendHtmlEmailAsync(email, $"Subject:Test",
                                       pathToFile,
                                       "",
                                       Config.GetSection("EmailAddress").Value,
                                       "",
                                       Config.GetSection("SmtpServer").Value,
                                       int.Parse(Config.GetSection("SmtpServerPort").Value));
                        }
                    }
                    
                     break;
                 }
                 catch (IOException)
                 {
                     Thread.Sleep(1000);
                 }
             }

            return pathToFile;
        }



        public void GenerateNoSqlFromSql()
        {
            GenerateGeneric(_repo.Get<ApplicationUser>());
            GenerateGeneric(_repo.Get<Account>());
            GenerateGeneric(_repo.Get<Language>());
            GenerateGeneric(_repo.Get<Location>());
            GenerateGeneric(_repo.Get<LocationField>());
            GenerateGeneric(_repo.GetMongo<MongoResult>("Results",null));
            GenerateGeneric(_repo.Get<Site>());
            GenerateGeneric(_repo.Get<SiteUser>());
            GenerateGeneric(_repo.Get<SureTrendVersionInfo>());
            GenerateGeneric(_repo.Get<Surface>());
            GenerateGeneric(_repo.Get<Plan>());
            GenerateGeneric(_repo.Get<PlanLocationMap>());
            GenerateGeneric(_repo.Get<PlanRandomizeSetting>());
            GenerateGeneric(_repo.Get<LanguageResource>());
            GenerateGeneric(_repo.Get<LanguageValue>());
            GenerateGeneric(_repo.Get<LanguageUserValue>());
            GenerateGeneric(_repo.Get<UserSetting>());
            GenerateGeneric(_repo.Get<UserAccount>());
            GenerateGeneric(_repo.Get<RegisterUnitToken>());
            GenerateGeneric(_repo.Get<Unit>());
            GenerateGeneric(_repo.Get<UserRolePermission>());
            GenerateGeneric(_repo.Get<ReportSchedule>());
            GenerateGeneric(_repo.Get<ReportScheduleSite>());
            GenerateGeneric(_repo.Get<UserFunctionRole>());
          

      
       
        }

        public void GenerateGeneric<T>(IEnumerable<T> entity) where T : class
        {
            //using (var db = new LiteRepository(@"filename =.\wwwroot\database\hygienaDatabase.db;"))
            //{
            //    foreach (var value in entity)
            //        using (var trans1 = db.BeginTrans())
            //        {
            //            db.Upsert(value);
            //            trans1.Commit();
            //        }
            //}
        }

        #endregion
    }
}