using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Plans")]
    public class Plan
    {
        public Plan()
        {
            PlanLocations = new List<PlanLocation>();
            PlanUnits = new List<PlanUnit>();
        }

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Test Plan Id is required")]
        public virtual Guid PlanId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Plan name is required")]
        public virtual string PlanName { get; set; }

        [Column(TypeName = "bit")]
        [Required(ErrorMessage = "Active flag is required")]
        public virtual bool Active { get; set; }

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
        [Required(ErrorMessage = "Is Random flag is required")]
        public virtual bool IsRandom { get; set; }

        [Column(TypeName = "bit")]
        [Required(ErrorMessage = "Prevent Repeat flag is required")]
        public virtual bool PreventRepeat { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Quota is required")]
        public virtual int Quota { get; set; }

        public IEnumerable<PlanLocation> PlanLocations { get; set; }
        public IEnumerable<PlanUnit> PlanUnits { get; set; }
    }
}