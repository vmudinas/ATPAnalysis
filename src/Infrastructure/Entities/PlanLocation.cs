using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("PlanLocation")]
    public class PlanLocation
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual Guid Id { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Plan Id is required")]
        public virtual Guid PlanId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Location Id is required")]
        public virtual Guid LocationId { get; set; }

        [Column(TypeName = "bit")]
        [Required(ErrorMessage = "Is required is a required field")]
        public virtual bool IsRequired { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Display order is required")]
        public virtual int DisplayOrder { get; set; }
    }
}
