using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    [Table("Languages")]
    public class Language
    {
        [Key]
        [BsonId]
        [Column(TypeName = "tinyint")]
        [Required(ErrorMessage = "Language Id is required")]
 
        public virtual byte LanguageId { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        public virtual string LanguageName { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string CultureInfo { get; set; }
    }
}