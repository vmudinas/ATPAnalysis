using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.MongoEntities
{
    [Table("Languages")]
    public class LanguageMongo
    {

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual ObjectId Id { get; set; }

        [Required(ErrorMessage = "Language Id is required")] 
        public virtual string LanguageId { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        public virtual string LanguageName { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string CultureInfo { get; set; }
    }
}