using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.UserModel.Models.AccountViewModels
{
    public class RoleSelectList
    {
        [Required]
        [BsonId]
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}