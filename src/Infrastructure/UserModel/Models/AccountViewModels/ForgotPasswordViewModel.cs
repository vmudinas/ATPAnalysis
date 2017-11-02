using System.ComponentModel.DataAnnotations;

namespace Infrastructure.UserModel.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}