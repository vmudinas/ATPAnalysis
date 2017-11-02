using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    [Table("Surfaces")]
    public class Surface
    {
        public Surface()
        {
            Locations = new List<Location>();
        }

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Surface Id is required")]
        public virtual Guid SurfaceId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }


        [MaxLength(250)]
        [StringLength(250)]
        public virtual string SurfaceName { get; set; }


        [MaxLength(4000)]
        [StringLength(4000)]
        public virtual string Notes { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? CreatedDate { get; set; }


        [MaxLength(250)]
        [StringLength(250)]
        public virtual string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ModifiedDate { get; set; }


        [MaxLength(250)]
        [StringLength(250)]
        public virtual string ModifiedBy { get; set; }

        [Column(TypeName = "bit")]
        public virtual bool? Status { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}