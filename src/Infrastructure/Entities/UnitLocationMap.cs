using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("UnitLocationMap")]
    public class UnitLocationMap
    {
        [Key]
        [BsonId]
        [ForeignKey("FK_UnitLocationMap_Units")]
        [Column(TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Unit No is required")]
        public virtual int UnitNo { get; set; }

        [Key]
        [ForeignKey("FK_UnitLocationMap_Locations")]
        [Column(TypeName = "uniqueidentifier", Order = 2)]
        [Required(ErrorMessage = "Location Id is required")]
        public virtual Guid LocationId { get; set; }

        [Column(TypeName = "int")]
        public virtual int? DisplayOrder { get; set; }
    }
}