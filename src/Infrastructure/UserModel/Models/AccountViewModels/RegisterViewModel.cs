using System.ComponentModel.DataAnnotations;

namespace Infrastructure.UserModel.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [Display(Name = "Account Email")]
        public string AccountEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        [StringLength(50)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(250)]
        public string Name { get; set; }


        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "CountryCode")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "DistrictState")]
        public string DistrictState { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Site")]
        public string Site { get; set; }
    }
}