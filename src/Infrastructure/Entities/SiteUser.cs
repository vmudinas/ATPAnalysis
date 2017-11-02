using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("SiteUsers")]
    public class SiteUser
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Site User Id is required")]
        public virtual Guid SiteUserId { get; set; }

        public virtual Guid? SiteId { get; set; }

        public virtual Guid? UserId { get; set; }
    }
}