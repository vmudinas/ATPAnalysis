using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.UserModel.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("UserSettings")]
    public class UserSetting
    {
        [Key]
        [BsonId]
        [Required(ErrorMessage = "User Setting Id is required")]
        public virtual Guid UserSettingId { get; set; }

        public virtual Language Language { get; set; }
        public ApplicationUser User { get; set; }

        public virtual string ResultGridSchema { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? ResultsPeriod { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ResultsDateFrom { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ResultsDateTo { get; set; }


        [Column(TypeName = "tinyint")]
        public virtual byte? DashType { get; set; }

        [Column(TypeName = "tinyint")]
        public virtual byte? DashPeriod { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? DashDateFrom { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? DashDateTo { get; set; }

        [MaxLength(25)]
        [StringLength(25)]
        public virtual string ReportScheduleCurrentView { get; set; }
    }
}