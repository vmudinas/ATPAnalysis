using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    [Table("PlanHistory")]
    public class PlanHistory
    {
        public PlanHistory()
        {
            PlanHistoryResults = new List<PlanHistoryResult>();
        }

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual Guid Id { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Plan Id is required")]
        public virtual Guid PlanId { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Start date is required")]
        public virtual DateTime StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? EndDate { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Quota is required")]
        public virtual int Quota { get; set; }

        public IEnumerable<PlanHistoryResult> PlanHistoryResults { get; set; }
    }
}
