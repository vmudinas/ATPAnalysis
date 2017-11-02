using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;

namespace Infrastructure.BusinessLogic
{
    public class ResultLogic
    {
        public IEnumerable<ClientResult> GetResults(IEnumerable<MongoResult> result, DateTime fromUtc, DateTime toUtc)
        {
            return result.Where(r => r.ResultDate >= fromUtc && r.ResultDate <= toUtc).Select(r => new ClientResult
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
            });
        }
    }
}