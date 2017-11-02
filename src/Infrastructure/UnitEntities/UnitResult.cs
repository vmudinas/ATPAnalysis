using System;

namespace Infrastructure.UnitEntities
{
    public class UnitResult
    {
        public virtual Guid ResultId { get; set; }

        public virtual Guid AccountId { get; set; }

        public virtual Guid SiteId { get; set; }

        public virtual DateTime ResultDate { get; set; }

        public virtual int UnitNo { get; set; }

        public virtual string UnitName { get; set; }

        public virtual string UnitType { get; set; }

        public virtual string UnitSoftware { get; set; }

        public virtual string UserName { get; set; }

        public virtual string PlanName { get; set; }

        public virtual string LocationName { get; set; }

        public virtual string GroupName { get; set; }

        public virtual string SurfaceName { get; set; }

        public virtual int? Rank { get; set; }

        public virtual string Zone { get; set; }

        public virtual string DeviceName { get; set; }

        public virtual string DeviceCategory { get; set; }

        public virtual string SampleType { get; set; }

        public virtual int? IncubationTime { get; set; }

        public virtual int? ActualIncubationTime { get; set; }

        public virtual int? Dilution { get; set; }

        public virtual string DeviceUOM { get; set; }

        public virtual int? DeviceTemperature { get; set; }

        public virtual int? UnitAngle { get; set; }

        public virtual string ControlName { get; set; }

        public virtual DateTime? ControlExpirationDate { get; set; }

        public virtual DateTime? ControlModifiedDate { get; set; }

        public virtual string ControlModifiedBy { get; set; }

        public virtual string ControlLotNumber { get; set; }

        public virtual int? Lower { get; set; }

        public virtual int? Upper { get; set; }

        public virtual bool? Result { get; set; }

        public virtual int RLU { get; set; }

        public virtual int RawADCOutput { get; set; }

        public virtual byte? TestState { get; set; }

        public virtual byte? RepeatedTest { get; set; }

        public virtual string CorrectiveAction { get; set; }

        public virtual Guid? CorrectedTest { get; set; }

        public virtual int WarningId { get; set; }

        public virtual string Notes { get; set; }

        public virtual int? RoomNumber { get; set; }

        public virtual string Personnel { get; set; }

        public virtual string CustomField1 { get; set; }

        public virtual string CustomField2 { get; set; }

        public virtual string CustomField3 { get; set; }

        public virtual string CustomField4 { get; set; }

        public virtual string CustomField5 { get; set; }

        public virtual string CustomField6 { get; set; }

        public virtual string CustomField7 { get; set; }

        public virtual string CustomField8 { get; set; }

        public virtual string CustomField9 { get; set; }

        public virtual string CustomField10 { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual bool? IsDeleted { get; set; }
    }
}