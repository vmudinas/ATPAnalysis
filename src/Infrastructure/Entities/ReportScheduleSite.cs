using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("ReportScheduleSites")]
    public class ReportScheduleSite
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "ReportScheduleSite Id is required")]
        public virtual Guid Id { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Site Id is required")]
        public virtual Guid SiteId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Schedule Id is required")]
        public virtual Guid ScheduleId { get; set; }
    }
}