using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infrastructure.Entities
{
    [Table("Accounts")]
    public class AccountMongo
    {
        public AccountMongo()
        {
            Sites = new List<Site>();
        }

        [Key]
        [BsonId]
        [Column(TypeName = "uniqueidentifier")]
        [Required(ErrorMessage = "Account Id is required")]
        public virtual Guid AccountId { get; set; }


        [MaxLength(20)]
        [StringLength(20)]
        public virtual string ERPCustomerId { get; set; }


        [MaxLength(250)]
        [StringLength(250)]
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }


        [MaxLength(100)]
        [StringLength(100)]
        public virtual string Address { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string Municipality { get; set; }


        [MaxLength(100)]
        [StringLength(100)]
        [Required(ErrorMessage = "City is required")]
        public virtual string City { get; set; }


        [MaxLength(10)]
        [StringLength(10)]
        public virtual string DistrictState { get; set; }


        [MaxLength(20)]
        [StringLength(20)]
        public virtual string PostalCode { get; set; }


        [MaxLength(20)]
        [StringLength(20)]
        [Required(ErrorMessage = "Country Code is required")]
        public virtual string CountryCode { get; set; }


        [MaxLength(256)]
        [StringLength(256)]
        [Required(ErrorMessage = "Email is required")]
        public virtual string Email { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        public virtual string Phone { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Created Date is required")]
        public virtual DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "bit")]
        public virtual bool? IsActive { get; set; }

        public IEnumerable<Site> Sites { get; set; }
    }
}