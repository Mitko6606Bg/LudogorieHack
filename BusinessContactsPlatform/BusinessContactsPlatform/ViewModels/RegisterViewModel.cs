using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password don't match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Graduated { get; set; }
        public string? Work { get; set; }
        public string? Experience { get; set; }
        public string? helpINeed { get; set; }
        public string? helpIOffere { get; set; }

        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
}
