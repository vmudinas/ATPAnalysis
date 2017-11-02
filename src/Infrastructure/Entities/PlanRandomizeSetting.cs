using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("PlanRandomizeSettings")]
    public class PlanRandomizeSetting
    {
        [Key]
        [ForeignKey("PlanId")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Test Plan Id is required")]
        [BsonId]
        public virtual Guid PlanId { get; set; }

        [Column(TypeName = "int")]
        public virtual int? NoOfLocations { get; set; }

        [Column(TypeName = "bit")]
        public virtual bool? IsPercentage { get; set; }

        [Column(TypeName = "bit")]
        public virtual bool? RiskRanking { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank1 { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank2 { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank3 { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank4 { get; set; }

        [Column(TypeName = "int")]
        public virtual int? Rank5 { get; set; }

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

        public ICollection<Plan> Plan { get; set; }
    }
}