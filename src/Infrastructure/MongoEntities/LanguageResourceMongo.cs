using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.MongoEntities
{
    [Table("LanguageResource")]
    public class LanguageResourceMongo
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual ObjectId Id { get; set; }

        [Required(ErrorMessage = "Resource Id is required")]
        public int ResourceValueId { get; set; }

        [MaxLength(1000)]
        [StringLength(1000)]
        public string LogicName { get; set; }
    }
}