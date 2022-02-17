using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100,ErrorMessage = "Max length is 100 symbols")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Max length is 100 symbols")]
        public string? LastName { get; set; }

        [Url]
        public string? PhotoURL { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}
