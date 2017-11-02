using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("UserAccounts")]
    public class UserAccount
    {
        [Key]
        [BsonId]
        [Required(ErrorMessage = "User Account Id is required")]
        public virtual Guid UserAccountId { get; set; }

        public virtual Guid AccountId { get; set; }
        public virtual Guid UserId { get; set; }
    }
}