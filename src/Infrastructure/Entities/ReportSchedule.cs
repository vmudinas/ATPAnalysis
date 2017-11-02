using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("ReportSchedules")]
    public class ReportSchedule
    {
        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Schedule Id is required")]
        public virtual Guid ScheduleId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "StartDate  is required")]
        public virtual DateTime StartDate { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "EndDate  is required")]
        public virtual DateTime EndDate { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Report Id is required")]
        public virtual Guid ReportId { get; set; }


        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "User Id is required")]
        public virtual Guid UserId { get; set; }


        [Column(TypeName = "tinyint")]
        [Required(ErrorMessage = "Report Period is required")]
        public virtual byte ReportPeriod { get; set; }


        [MaxLength(256)]
        [StringLength(256)]
        [Required(ErrorMessage = "Schedule Title is required")]
        public virtual string ScheduleTitle { get; set; }


        [MaxLength(1024)]
        [StringLength(1024)]
        [Required(ErrorMessage = "Email List is required")]
        public virtual string EmailList { get; set; }


        [MaxLength(128)]
        [StringLength(128)]
        public virtual string RecurrenceRule { get; set; }


        [Column(TypeName = "datetime")]
        public virtual DateTime? LastSent { get; set; }
        

        [Column(TypeName = "datetime")]
        public virtual DateTime? NextSend { get; set; }
    }
}