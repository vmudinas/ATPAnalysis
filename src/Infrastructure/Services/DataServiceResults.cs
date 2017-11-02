using System;
using System.Collections.Generic;
using System.Linq;
using DevExtreme.AspNet.Data;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;
using MongoDB.Bson;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Infrastructure.Services
{


    public partial class DataService : IDataService
    {
        public IEnumerable<MongoResult> GetAllResults(Account account)
        {
            return account == null ? null : _repo.GetMongo<MongoResult>("Results",x => x.AccountId == account.AccountId.ToString()).ToList();
        }

        public IEnumerable<ClientResult> GetResults(DateTime fromUtc, DateTime toUtc, ClientAccount account)
        {
            if (account == null)
                return null;

            var siteList = account.Sites.Select(s => s.SiteId.ToString()).ToList();
            
             var results =   _repo.GetMongoIQuerable<MongoResult>("Results").Where(x => fromUtc <= x.ResultDate && x.ResultDate <= toUtc &&
                                 x.AccountId == account.AccountId.ToString()
                                 && siteList.Contains(x.SiteId))
                        .ToList();
     

            return results.Select(r => new ClientResult
            {
                ResultId = Guid.Parse(r.ResultId),
                AccountId = Guid.Parse(r.AccountId),
                SiteId = Guid.Parse(r.SiteId),
                ResultDate = r.ResultDate,
                UnitNo = r.UnitNo,
                UnitName = r.UnitName,
                UnitType = r.UnitType,
                UserName = r.UserName,
                UnitSoftware = r.UnitSoftware,
                PlanName = r.PlanName,
                LocationName = r.LocationName,
                GroupName = r.GroupName,
                SurfaceName = r.SurfaceName,
                Rank = r.Rank,
                Zone = r.Zone,
                DeviceName = r.DeviceName,
                DeviceCategory = r.DeviceCategory,
                SampleType = r.SampleType,
                IncubationTime = r.IncubationTime,
                ActualIncubationTime = r.ActualIncubationTime,
                Dilution = r.Dilution,
                DeviceUOM = r.DeviceUOM,
                DeviceTemperature = r.DeviceTemperature,
                UnitAngle = r.UnitAngle,
                ControlName = r.ControlName,
                ControlExpirationDate = r.ControlExpirationDate,
                ControlModifiedDate = r.ControlModifiedDate,
                ControlModifiedBy = r.ControlModifiedBy,
                ControlLotNumber = r.ControlLotNumber,
                Lower = r.Lower,
                Upper = r.Upper,
                RLU = r.RLU,
                RawADCOutput = r.RawADCOutput,
                TestState = r.TestState,
                RepeatedTest = r.RepeatedTest,
                CorrectiveAction = r.CorrectiveAction,
                CorrectedTest = r.CorrectedTest,
                WarningId = r.WarningId,
                Notes = r.Notes,
                RoomNumber = r.RoomNumber,
                Personnel = r.Personnel,
                CustomField1 = r.CustomField1,
                CustomField2 = r.CustomField2,
                CustomField3 = r.CustomField3,
                CustomField4 = r.CustomField4,
                CustomField5 = r.CustomField5,
                CustomField6 = r.CustomField6,
                CustomField7 = r.CustomField7,
                CustomField8 = r.CustomField8,
                CustomField9 = r.CustomField9,
                CustomField10 = r.CustomField10,
                CreatedDate = r.CreatedDate,
                CreatedBy = r.CreatedBy,
                ModifiedDate = r.ModifiedDate,
                ModifiedBy = r.ModifiedBy,
                IsDeleted = r.IsDeleted
            }).ToList().OrderByDescending(r => r.ResultDate);
        }

        public int GetPagedResultsCount(DateTime fromUtc, DateTime toUtc, DataSourceLoadOptions options, ClientAccount account)
        {

            if (options.Sort == null && options.Filter == null)
            {
                return 0;
            }

            if (account == null)
                return 0;

            var siteList = account.Sites.Select(s => s.SiteId.ToString()).ToList();

       
            var results = _repo.GetMongoCount<MongoResult>("Results",
            x => fromUtc <= x.ResultDate && x.ResultDate <= toUtc
                        && x.AccountId == account.AccountId.ToString()
                        && siteList.Contains(x.SiteId), GenerateMongoFilter(options));


            return results;
        }

        private static string ParseComparator(string pComparator)
        {
            if (pComparator == "=")
                return "$eq:";
            else if (pComparator == "contains")
                return "contains";
            else if (pComparator == "endswith")
                return "endswith";
            else if (pComparator == "startswith")
                return "startswith";
            else if (pComparator == ">")
                return "$gt:";
            else if (pComparator == ">=")
                return "$gte:";
            else if (pComparator == "<")
                return "$lt:";
            else if (pComparator == "<=")
                return "$lte:";
            else if (pComparator == "<>")
                return "$ne:";

            return "";
        }
        private string BuildFilter(string value, string filterBuilder)
        {
            try
            {
                string removedBreaks = value.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                var commaSplit = removedBreaks.Replace("[", "").Replace("]", "").Replace(" ", "").Trim(',').Split(',');

                var filterValue = commaSplit[2];
                var fieldName = FirstLetterToUpper(commaSplit[0].Replace("\"", ""));
                filterValue = filterValue.Replace("\"", "").Replace("\"", "");

                if (fieldName.ToLower().Equals("rlu"))
                {
                    fieldName = "RLU";
                }
                PropertyInfo propertyInfo = typeof(MongoResult).GetProperty(fieldName);
                if (propertyInfo.PropertyType == typeof(string))
                {

                    filterValue = $"\"{filterValue}\"";
                }

                var queryOperator = ParseComparator(commaSplit[1].Replace("\"", ""));

                if (queryOperator.Equals("contains"))
                {
                    filterBuilder = $"{filterBuilder}{{{fieldName}:/.*{filterValue.Replace("\"", "")}.*/}},";
                }
                else if (queryOperator.Equals("endswith"))
                {
                    filterBuilder = $"{filterBuilder}{{{fieldName}:/.*{filterValue.Replace("\"", "")}./}},";
                }
                else if (queryOperator.Equals("startswith"))
                {
                    filterBuilder = $"{filterBuilder}{{{fieldName}:/.{filterValue.Replace("\"", "")}.*/}},";
                }
                else
                {

                    filterBuilder = $"{filterBuilder}{{{fieldName}:{{{queryOperator}{filterValue}}}}},";
                }

                return filterBuilder;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private string LoopFilter(string filterBuilder, DataSourceLoadOptions options)
        {
            foreach (var value in options.Filter.AsQueryable().AsEnumerable())
            {

                if (!value.Equals("and"))
                {
                    var multiFilter = value.ToString().Replace("\"", "").Split("and");
                    if (multiFilter.Length > 1)
                    {
                        foreach (var multiFilterValue in multiFilter)
                        {
                            if (!multiFilterValue.Equals("and"))
                            {
                                filterBuilder = BuildFilter(multiFilterValue.ToString(), filterBuilder);
                            }
                        }
                    }
                    else
                    {
                        filterBuilder = BuildFilter(value.ToString(), filterBuilder);
                    }
                      
                }
            }
            return filterBuilder;
        }
        private string  GenerateMongoFilter(DataSourceLoadOptions options)
        {
           var filterBuilder = "";

            if (options.Filter != null)
            {

                if (options.Filter.Count == 3)
                 {
                    var checkFilter = options.Filter.AsQueryable().ToDynamicArray();
                    if (!checkFilter[1].Equals("and"))
                    {
                       
                        return $"{{ $and: [{ BuildFilter($"{checkFilter[0]},{checkFilter[1]},{checkFilter[2]}", filterBuilder).TrimEnd(',')}] }}";
                    }
                   
                }
                return $"{{ $and: [{LoopFilter(filterBuilder, options).TrimEnd(',')}] }}";
             
            }

            return "";

        }
        public object GetPagedResults(DateTime fromUtc, DateTime toUtc, DataSourceLoadOptions options, ClientAccount account)
        {
                    

            if (options.Sort == null && options.Filter == null)
            {
                return new List<MongoResult>();
            }       


            if (account == null)
                return null;
            var siteList = account.Sites.Select(s => s.SiteId.ToString()).ToList();

              var results =  _repo.GetMongoResults("Results",
                        x => fromUtc <= x.ResultDate && x.ResultDate <= toUtc &&
                             x.AccountId == account.AccountId.ToString()
                             && siteList.Contains(x.SiteId), GetSort(options.Sort), GenerateMongoFilter(options), options.Skip, options.Take);
           

            options.IsCountQuery = true;

            return results;         

            
        }
        private string GetSort(SortingInfo[] sortingInfo)
        {
            var sort = "";



            foreach (var sortValue in sortingInfo)
            {
                if (sortValue.Desc)
                {

                    sort = $"{sort}{FirstLetterToUpper(sortValue.Selector)}:-1,";
                }
                else
                {
                    sort = $"{sort}{FirstLetterToUpper(sortValue.Selector)}:0,";
                }
            }

          return  sort = $"{{{sort.TrimEnd(',')}}}";

        }
        private string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
        
        public void UpdateResult(ClientResult cr)
        {
            var r = new MongoResult
            {
                Id = new ObjectId(cr.ResultId.ToString().Replace("-", "")),
                ResultId = cr.ResultId.ToString(),
                AccountId = cr.AccountId.ToString(),
                SiteId = cr.SiteId.ToString(),
                ResultDate = cr.ResultDate,
                UnitNo = cr.UnitNo,
                UnitName = cr.UnitName,
                UnitType = cr.UnitType,
                UserName = cr.UserName,
                UnitSoftware = cr.UnitSoftware,
                PlanName = cr.PlanName,
                LocationName = cr.LocationName,
                GroupName = cr.GroupName,
                SurfaceName = cr.SurfaceName,
                Rank = cr.Rank,
                Zone = cr.Zone,
                DeviceName = cr.DeviceName,
                DeviceCategory = cr.DeviceCategory,
                SampleType = cr.SampleType,
                IncubationTime = cr.IncubationTime,
                ActualIncubationTime = cr.ActualIncubationTime,
                Dilution = cr.Dilution,
                DeviceUOM = cr.DeviceUOM,
                DeviceTemperature = cr.DeviceTemperature,
                UnitAngle = cr.UnitAngle,
                ControlName = cr.ControlName,
                ControlExpirationDate = cr.ControlExpirationDate,
                ControlModifiedDate = cr.ControlModifiedDate,
                ControlModifiedBy = cr.ControlModifiedBy,
                ControlLotNumber = cr.ControlLotNumber,
                Lower = cr.Lower,
                Upper = cr.Upper,
                RLU = cr.RLU,
                RawADCOutput = cr.RawADCOutput,
                TestState = cr.TestState,
                RepeatedTest = cr.RepeatedTest,
                CorrectiveAction = cr.CorrectiveAction,
                CorrectedTest = cr.CorrectedTest,
                WarningId = cr.WarningId,
                Notes = cr.Notes,
                RoomNumber = cr.RoomNumber,
                Personnel = cr.Personnel,
                CustomField1 = cr.CustomField1,
                CustomField2 = cr.CustomField2,
                CustomField3 = cr.CustomField3,
                CustomField4 = cr.CustomField4,
                CustomField5 = cr.CustomField5,
                CustomField6 = cr.CustomField6,
                CustomField7 = cr.CustomField7,
                CustomField8 = cr.CustomField8,
                CustomField9 = cr.CustomField9,
                CustomField10 = cr.CustomField10,
                CreatedDate = cr.CreatedDate,
                CreatedBy = cr.CreatedBy,
                ModifiedDate = cr.ModifiedDate,
                ModifiedBy = cr.ModifiedBy,
                IsDeleted = cr.IsDeleted
            };
            _repo.UpdateSave(r);
        }

        #region Moved from SandBox service 


        public IEnumerable<MongoResult> GetResultsByUnit(int unitNo)
        {
            return _repo.GetMongoIQuerable<MongoResult>("Results").Where(x => x.UnitNo == unitNo);
        }

        public IEnumerable<ClientUnitResult> AddTestResults(ResultsItem unitResultsItem)
        {
            var lstDbResultStamps = GetResultStamps(unitResultsItem.UnitNo, unitResultsItem.AccountId,
                unitResultsItem.SiteId);
            var returnList = new List<ClientUnitResult>();
            Result r;

            foreach (var ur in unitResultsItem.Payload)
                try
                {
                    r = GetResultFromUnitResult(ur, unitResultsItem.UnitNo, unitResultsItem.AccountId,
                        unitResultsItem.SiteId);

                    if (lstDbResultStamps != null && lstDbResultStamps.Count > 0 &&
                        lstDbResultStamps.Exists(rs => rs.Id == r.ResultId))
                    {
                           if (r.ModifiedDate != DateTime.MinValue &&
                            lstDbResultStamps.Exists(rs => rs.ModDate < r.ModifiedDate))
                            _repo.UpdateSave(r);
                    }
                    else
                    {
                        _repo.AddSave(r);
                    }
                }
                catch (Exception ex)
                {
                    returnList.Add(
                        new ClientUnitResult
                        {
                            AccountId = unitResultsItem?.AccountId,
                            SiteId = unitResultsItem?.SiteId,
                            ResultId = ur?.ResultId,
                            UnitId = unitResultsItem?.UnitNo,
                            Error = ex?.Message + ex?.InnerException
                        }
                    );
                }

            return returnList;
        }

        private static Result GetResultFromUnitResult(UnitEntities.UnitResult urIn, int unitNo, Guid acctId, Guid siteId)
        {
            var retRes = new Result
            {
                AccountId = acctId,
                ActualIncubationTime = urIn.ActualIncubationTime,
                ControlExpirationDate = urIn.ControlExpirationDate,
                ControlLotNumber = urIn.ControlLotNumber,
                ControlModifiedBy = urIn.ControlModifiedBy,
                ControlModifiedDate = urIn.ControlModifiedDate,
                ControlName = urIn.ControlName,
                CorrectedTest = urIn.CorrectedTest,
                CorrectiveAction = urIn.CorrectiveAction,
                CreatedBy = urIn.CreatedBy,
                CreatedDate = urIn.CreatedDate,
                CustomField1 = urIn.CustomField1,
                CustomField2 = urIn.CustomField2,
                CustomField3 = urIn.CustomField3,
                CustomField4 = urIn.CustomField4,
                CustomField5 = urIn.CustomField5,
                CustomField6 = urIn.CustomField6,
                CustomField7 = urIn.CustomField7,
                CustomField8 = urIn.CustomField8,
                CustomField9 = urIn.CustomField9,
                CustomField10 = urIn.CustomField10,
                DeviceCategory = urIn.DeviceCategory,
                DeviceName = urIn.DeviceName,
                DeviceTemperature = urIn.DeviceTemperature,
                DeviceUOM = urIn.DeviceUOM,
                Dilution = urIn.Dilution,
                GroupName = urIn.GroupName,
                IncubationTime = urIn.IncubationTime,
                IsDeleted = urIn.IsDeleted,
                LocationName = urIn.LocationName,
                Lower = urIn.Lower,
                ModifiedBy = urIn.ModifiedBy,
                ModifiedDate = urIn.ModifiedDate,
                Notes = urIn.Notes,
                Personnel = urIn.Personnel,
                Rank = urIn.Rank,
                RawADCOutput = urIn.RawADCOutput,
                RepeatedTest = urIn.RepeatedTest,
                ResultDate = urIn.ResultDate,
                ResultId = urIn.ResultId,
                RLU = urIn.RLU,
                RoomNumber = urIn.RoomNumber,
                SampleType = urIn.SampleType,
                SiteId = siteId,
                SurfaceName = urIn.SurfaceName,
                PlanName = urIn.PlanName,
                TestState = urIn.TestState,
                UnitAngle = urIn.UnitAngle,
                UnitName = urIn.UnitName,
                UnitNo = unitNo,
                UnitSoftware = urIn.UnitSoftware,
                UnitType = urIn.UnitType,
                Upper = urIn.Upper,
                UserName = urIn.UserName,
                WarningId = urIn.WarningId,
                Zone = urIn.Zone
            };
            return retRes;
        }

        private List<ResultStamp> GetResultStamps(int unitNo, Guid acctId, Guid siteId)
        {
            return (from r in _repo.GetMongo<MongoResult>("Results",r => r.UnitNo == unitNo && r.AccountId == acctId.ToString() && r.SiteId == siteId.ToString())
                    select new ResultStamp {Id = Guid.Parse(r.ResultId), ModDate = r.ModifiedDate}).ToList();
        }

        protected class ResultStamp
        {
            public Guid Id { get; set; }
            public DateTime? ModDate { get; set; } 
        }

        #endregion

        #region Sync Results

        public CloudSyncReturn AddResultCloudSync(ClientCloudSyncResult cloudSyncData)
        {

            var addedUpdatedItemResults = new CloudSyncReturn();
            var resultList = cloudSyncData.Payload.Select(value => new Result
                {
                    AccountId = cloudSyncData.AccountId,
                    ActualIncubationTime = value.ActualIncubationTime,
                    ControlExpirationDate = value.ControlExpirationDate,
                    ControlLotNumber = value.ControlLotNumber,
                    ControlModifiedBy = value.ControlModifiedBy,
                    ControlModifiedDate = value.ControlModifiedDate,
                    ControlName = value.ControlName,
                    CorrectedTest = value.CorrectedTest,
                    CorrectiveAction = value.CorrectiveAction,
                    CreatedBy = value.CreatedBy,
                    CreatedDate = value.CreatedDate,
                    CustomField1 = value.CustomField1,
                    CustomField2 = value.CustomField2,
                    CustomField3 = value.CustomField3,
                    CustomField4 = value.CustomField4,
                    CustomField5 = value.CustomField5,
                    CustomField6 = value.CustomField6,
                    CustomField7 = value.CustomField7,
                    CustomField8 = value.CustomField8,
                    CustomField9 = value.CustomField9,
                    CustomField10 = value.CustomField10,
                    DeviceCategory = value.DeviceCategory,
                    DeviceName = value.DeviceName,
                    DeviceTemperature = value.DeviceTemperature,
                    DeviceUOM = value.DeviceUOM,
                    Dilution = value.Dilution,
                    GroupName = value.GroupName,
                    IncubationTime = value.IncubationTime,
                    IsDeleted = value.IsDeleted,
                    LocationName = value.LocationName,
                    Lower = value.Lower,
                    ModifiedBy = value.ModifiedBy,
                    ModifiedDate = value.ModifiedDate,
                    Notes = value.Notes,
                    Personnel = value.Personnel,
                    Rank = value.Rank,
                    RawADCOutput = value.RawADCOutput,
                    RepeatedTest = value.RepeatedTest,
                    ResultDate = value.ResultDate,
                    ResultId = value.ResultId,
                    RLU = value.RLU,
                    RoomNumber = value.RoomNumber,
                    SampleType = value.SampleType,
                    SiteId = cloudSyncData.SiteId,
                    SurfaceName = value.SurfaceName,
                    PlanName = value.PlanName,
                    TestState = value.TestState,
                    UnitAngle = value.UnitAngle,
                    UnitName = value.UnitName,
                    UnitNo = value.UnitNo,
                    UnitSoftware = value.UnitSoftware,
                    UnitType = value.UnitType,
                    Upper = value.Upper,
                    UserName = value.UserName,
                    WarningId = value.WarningId,
                    Zone = value.Zone
                })
                .ToList();
            var updatedCount = 0;
            var inserterCount = 0;
            try
            {

            foreach (var value in resultList)
            {
                if (!_repo.GetAll<MongoResult>().Any(x => x.ResultId == value.ResultId.ToString()))
                {
                    _repo.Add(value);
                    inserterCount++;
                }
                else
                {
                    _repo.Update(value);
                    updatedCount++;
                }
            }

            _repo.Save();
            addedUpdatedItemResults.ResultsAdded = inserterCount;
            addedUpdatedItemResults.ResultsUpdated = updatedCount;

            }
            catch (Exception ex)
            {
                addedUpdatedItemResults.ResultsAdded = 0;
                addedUpdatedItemResults.ResultsUpdated = 0;
                addedUpdatedItemResults.ResultsErrors =  1;
                addedUpdatedItemResults.ErrorList.Add($"{ex.Message} InnerException: {ex.InnerException?.Message}");
            }
            return addedUpdatedItemResults;
        }

        public CloudSyncReturn AddResultCloudSyncMongo(ClientCloudSyncResult cloudSyncData)
        {
          
            var addedUpdatedItemResults = new CloudSyncReturn();
            var resultList = cloudSyncData.Payload.Select(value => new MongoResult
            {
                AccountId = cloudSyncData.AccountId.ToString(),
                ActualIncubationTime = value.ActualIncubationTime,
                ControlExpirationDate = value.ControlExpirationDate,
                ControlLotNumber = value.ControlLotNumber,
                ControlModifiedBy = value.ControlModifiedBy,
                ControlModifiedDate = value.ControlModifiedDate,
                ControlName = value.ControlName,
                CorrectedTest = value.CorrectedTest,
                CorrectiveAction = value.CorrectiveAction,
                CreatedBy = value.CreatedBy,
                CreatedDate = value.CreatedDate,
                CustomField1 = value.CustomField1,
                CustomField2 = value.CustomField2,
                CustomField3 = value.CustomField3,
                CustomField4 = value.CustomField4,
                CustomField5 = value.CustomField5,
                CustomField6 = value.CustomField6,
                CustomField7 = value.CustomField7,
                CustomField8 = value.CustomField8,
                CustomField9 = value.CustomField9,
                CustomField10 = value.CustomField10,
                DeviceCategory = value.DeviceCategory,
                DeviceName = value.DeviceName,
                DeviceTemperature = value.DeviceTemperature,
                DeviceUOM = value.DeviceUOM,
                Dilution = value.Dilution,
                GroupName = value.GroupName,
                IncubationTime = value.IncubationTime,
                IsDeleted = value.IsDeleted,
                LocationName = value.LocationName,
                Lower = value.Lower,
                ModifiedBy = value.ModifiedBy,
                ModifiedDate = value.ModifiedDate,
                Notes = value.Notes,
                Personnel = value.Personnel,
                Rank = value.Rank,
                RawADCOutput = value.RawADCOutput,
                RepeatedTest = value.RepeatedTest,
                ResultDate = value.ResultDate,
                ResultId = value.ResultId.ToString(),
                Id = new ObjectId(value.ResultId.ToString().Replace("-", "")),
                RLU = value.RLU,
                RoomNumber = value.RoomNumber,
                SampleType = value.SampleType,
                SiteId = cloudSyncData.SiteId.ToString(),
                SurfaceName = value.SurfaceName,
                PlanName = value.PlanName,
                TestState = value.TestState,
                UnitAngle = value.UnitAngle,
                UnitName = value.UnitName,
                UnitNo = value.UnitNo,
                UnitSoftware = value.UnitSoftware,
                UnitType = value.UnitType,
                Upper = value.Upper,
                UserName = value.UserName,
                WarningId = value.WarningId,
                Zone = value.Zone
            })
                .ToList();
        
            try
            {
               var dbStatus = _repo.UpdateResultMongoDb(resultList, "Results");

                addedUpdatedItemResults.ResultsAdded = unchecked((int)dbStatus.Inserted); 
                addedUpdatedItemResults.ResultsUpdated = unchecked((int)dbStatus.Updated);  

            }
            catch (Exception ex)
            {
                addedUpdatedItemResults.ResultsAdded = 0;
                addedUpdatedItemResults.ResultsUpdated = 0;
                addedUpdatedItemResults.ResultsErrors = 1;
                addedUpdatedItemResults.ErrorList.Add($"{ex.Message} InnerException: {ex.InnerException?.Message}");
            }
            return addedUpdatedItemResults;
        }

        #endregion
    }
}