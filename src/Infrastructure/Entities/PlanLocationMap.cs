using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("PlanLocationMap")]
    public class PlanLocationMap
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Map Id is required")]
        public virtual Guid MapId { get; set; }

        [ForeignKey("FK_PlanLocationMap_TestPlans")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Plan Id is required")]
        public virtual Guid PlanId { get; set; }

        [ForeignKey("FK_PlanLocationMap_Locations")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Location Id is required")]
        public virtual Guid LocationId { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        public virtual int DisplayOrder { get; set; }
    }
}