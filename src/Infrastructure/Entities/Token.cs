using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Tokens")]
    public class Token
    {
        [Key]
        [BsonId]
        public virtual Guid Id { get; set; }

        public virtual Guid UserId { get; set; }

        public virtual string Tokens { get; set; }

        public virtual string UnitSerial { get; set; }

        [Column(TypeName = "bit")]
        [Required(ErrorMessage = "Active flag is required")]
        public virtual bool Active { get; set; }


        [Column(TypeName = "datetime")]
        public virtual DateTime CreatedDate { get; set; }

    }
}
