using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infrastructure.MongoEntities
{
    [Table("LanguageUserValues")]
    public class LanguageUserValueMongo
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Id is required")]
        public virtual ObjectId Id { get; set; }

 
        [Required(ErrorMessage = "Language Resource Id is required")]
        public virtual string LanguageUserId { get; set; }


        public virtual string LanguageResourceId { get; set; }

        public virtual string LanguageId { get; set; }

        public virtual string AccountId { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        public virtual string Caption { get; set; }

        [MaxLength(1000)]
        [StringLength(1000)]
        public string ToolTip { get; set; }
    }
}