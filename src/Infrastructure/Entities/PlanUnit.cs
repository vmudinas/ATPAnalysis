using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("PlanUnit")]
    public class PlanUnit
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual Guid Id { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Plan Id is required")]
        public virtual Guid PlanId { get; set; }

        [BsonId]
        [Required(ErrorMessage = "Unit Number is required")]
        public virtual int UnitNo { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Quota is required")]
        public virtual int Quota { get; set; }
    }
}
