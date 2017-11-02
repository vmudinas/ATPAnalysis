using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("LanguageResource")]
    public class LanguageResource
    {
        [Key]
        [BsonId]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Resource Id is required")]
        public int ResourceValueId { get; set; }

        [MaxLength(1000)]
        [StringLength(1000)]
        public string LogicName { get; set; }
    }
}