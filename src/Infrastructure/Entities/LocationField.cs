using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("LocationFields")]
    public class LocationField
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier", Order = 1)]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }

        [Key]
        [Column(TypeName = "tinyint", Order = 2)]
        [Required(ErrorMessage = "Field Id is required")]
        public virtual byte FieldId { get; set; }

        [Column(TypeName = "tinyint")]
        [Required(ErrorMessage = "Field Type is required")]
        public virtual byte FieldType { get; set; }

        [Key]
        [Column(TypeName = "int", Order = 3)]
        [Required(ErrorMessage = "Device Usage Id is required")]
        public virtual int DeviceUsageId { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string FieldName { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string Caption { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string DataType { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ModifiedDate { get; set; }


        [MaxLength(250)]
        [StringLength(250)]
        public virtual string ModifiedBy { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Status { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? CollectedAt { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? TestState { get; set; }


        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string Validation { get; set; }
    }
}