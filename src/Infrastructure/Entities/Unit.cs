using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Units")]
    public class Unit
    {
        [BsonId]
        [MaxLength(250)]
        [StringLength(250)]
        [Key]
        public virtual Guid Id { get; set; }
        
        public virtual int? UnitNo { get; set; }

        public virtual Guid SiteId { get; set; }
        public virtual Guid AccountId { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Unit Name is required")]
        public virtual string UnitName { get; set; }
    }
}