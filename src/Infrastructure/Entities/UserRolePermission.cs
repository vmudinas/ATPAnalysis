using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [Table("UserRolePermissions")]
    public class UserRolePermission
    {
        [Key]
        [BsonId]
        public Guid RoleUserPermissionId { get; set; }

        public virtual IdentityRole Permission { get; set; }
        public virtual UserFunctionRole Role { get; set; }
    }
}