using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.MongoEntities;
using Infrastructure.ClientEntities;
using MongoDB.Bson;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {       
        public void MigrationLanguages()
        {     
            var addedUpdatedItemResults = new CloudSyncReturn();

       
            var getLanguageValue = _repo.GetWithLanguageLanguageResource();
            var mongoLanguageValueList = new List<LanguageValueMongo>();
            foreach (var languageValue in getLanguageValue)
            {
                var mongoLanguageResource = new LanguageValueMongo
                {
                    Id = new ObjectId(Guid.NewGuid().ToString().Replace("-", "")),
                    LanguageResourceId = languageValue.LanguageResource.ResourceValueId,
                    LanguageId = languageValue.Language.LanguageId.ToString(),
                    Caption = languageValue.Caption,
                    ToolTip = languageValue.ToolTip
                };
                mongoLanguageValueList.Add(mongoLanguageResource);
            }

            var languageValuesStatus = _repo.UpdateMongo(mongoLanguageValueList, "LanguageValues");

        }

        }   
 }
