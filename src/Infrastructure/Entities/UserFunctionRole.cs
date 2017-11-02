using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("UserRoles")]
    public class UserFunctionRole
    {
        [Key]
        [BsonId]
        [Required(ErrorMessage = "User Role Id is required")]
        public virtual Guid RoleId { get; set; }

        public string Role { get; set; }
        public string RoleDescription { get; set; }
        public Guid? AccountId { get; set; }
    }
}