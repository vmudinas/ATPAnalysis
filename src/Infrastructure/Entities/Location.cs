using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Locations")]
    public class Location
    {
        public Location()
        {
            PlanLocationMaps = new List<PlanLocationMap>();
            UnitLocationMaps = new List<UnitLocationMap>();
        }

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Location Id is required")]
        public virtual Guid LocationId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string LocationName { get; set; }


        [MaxLength(500)]
        [StringLength(500)]
        public virtual string Description { get; set; }

        [ForeignKey("FK_mLocation_mGroup")]
        [Column(TypeName = "uniqueidentifier")]
        public virtual Guid? GroupId { get; set; }

        [ForeignKey("FK_mLocation_mSurface")]
        [Column(TypeName = "uniqueidentifier")]
        public virtual Guid? SurfaceId { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Device Usage Id is required")]
        public virtual int DeviceUsageId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public virtual Guid? ControlId { get; set; }

        [Column(TypeName = "int")]
        public virtual int? IncubationTime { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? QualitativeQuantitative { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Lower { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Upper { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Rank is required")]
        public virtual int Rank { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public virtual Guid? ZoneID { get; set; }

        [Column(TypeName = "int")]
        public virtual int? DisplayOrder { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string CorrectiveAction { get; set; }


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
        public virtual bool? Status { get; set; }

        public IEnumerable<PlanLocationMap> PlanLocationMaps { get; set; }

        public IEnumerable<UnitLocationMap> UnitLocationMaps { get; set; }
    }
}