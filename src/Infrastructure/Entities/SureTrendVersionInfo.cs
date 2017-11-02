using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("SureTrendVersionInfo")]
    public class SureTrendVersionInfo
    {
        [Key]
        [BsonId]
        [MaxLength(100)]
        [StringLength(100)]
        public virtual string AppVersion { get; set; }


        [MaxLength(100)]
        [StringLength(100)]
        public virtual string DatabaseVersion { get; set; }
    }
}