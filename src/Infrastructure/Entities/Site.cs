using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Sites")]
    public class Site
    {
        [Key]
        [BsonId]
        public virtual Guid SiteId { get; set; }

        public virtual Guid AccountId { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        [Column(TypeName = "bit")]
        [Required(ErrorMessage = "Active flag is required")]
        public virtual bool Active { get; set; }
    }
}