using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("PlanHistoryResult")]
    public class PlanHistoryResult
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual Guid Id { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Plan History Id is required")]
        public virtual Guid PlanHistoryId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Result Id is required")]
        public virtual Guid ResultId { get; set; }
    }
}
