using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("LanguageAccountResource")]
    public class LanguageAccountResource
    {
        [Key]
        [BsonId]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Language Resource Id is required")]
        public virtual int LanguageAccountResourceId { get; set; }


        public virtual LanguageResource LanguageResource { get; set; }

        public virtual Language Language { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        public virtual string Caption { get; set; }

        [MaxLength(1000)]
        [StringLength(1000)]
        public string ToolTip { get; set; }
    }
}