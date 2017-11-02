using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("RegisterUnitTokens")]
    public class RegisterUnitToken
    {
        [Key]
        [BsonId]
        [Required(ErrorMessage = "Token Id is required")]
        public virtual Guid TokenId { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Token  is required")]
        public virtual string Token { get; set; }

        public virtual Guid? UnitId { get; set; }
        public virtual Guid? SiteId { get; set; }
        public virtual Guid? AccountId { get; set; }
        public virtual string UserId { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        public virtual string CreatorUserName { get; set; }

        public virtual bool IsCloudSync { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        public virtual string Name { get; set; }


    }
}