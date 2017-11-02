using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Results")]
    public class Result
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Result Id is required")]
        public virtual Guid ResultId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Site Id is required")]
        public virtual Guid SiteId { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Result Date is required")]
        public virtual DateTime ResultDate { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Unit No is required")]
        public virtual int UnitNo { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        [Required(ErrorMessage = "Unit Name is required")]
        public virtual string UnitName { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        public virtual string UnitType { get; set; }

        [MaxLength(30)]
        [StringLength(30)]
        public virtual string UserName { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        public virtual string UnitSoftware { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Plan Name is required")]
        public virtual string PlanName { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Location Name is required")]
        public virtual string LocationName { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string GroupName { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string SurfaceName { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string Zone { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        public virtual string DeviceName { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        public virtual string DeviceCategory { get; set; }

        [MaxLength(25)]
        [StringLength(25)]
        public virtual string SampleType { get; set; }

        [Column(TypeName = "int")]
        public virtual int? IncubationTime { get; set; }

        [Column(TypeName = "int")]
        public virtual int? ActualIncubationTime { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Dilution { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        public virtual string DeviceUOM { get; set; }

        [Column(TypeName = "int")]
        public virtual int? DeviceTemperature { get; set; }

        [Column(TypeName = "int")]
        public virtual int? UnitAngle { get; set; }

        [MaxLength(25)]
        [StringLength(25)]
        public virtual string ControlName { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ControlExpirationDate { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ControlModifiedDate { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        public virtual string ControlModifiedBy { get; set; }

        [MaxLength(25)]
        [StringLength(25)]
        public virtual string ControlLotNumber { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Lower { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Upper { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "RLU is required")]
        public virtual int RLU { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Raw ADC Output is required")]
        public virtual int RawADCOutput { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? TestState { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? RepeatedTest { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        public virtual string CorrectiveAction { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public virtual Guid? CorrectedTest { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Warning Id is required")]
        public virtual int WarningId { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string Notes { get; set; }

        [Column(TypeName = "int")]
        public virtual int? RoomNumber { get; set; }

        [MaxLength(256)]
        [StringLength(256)]
        public virtual string Personnel { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField1 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField2 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField3 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField4 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField5 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField6 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField7 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField8 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField9 { get; set; }

        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string CustomField10 { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? CreatedDate { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ModifiedDate { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string ModifiedBy { get; set; }

        [Column(TypeName = "bit")]
        public virtual bool? IsDeleted { get; set; }
    }
}