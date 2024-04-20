using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.Data.Entities
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Graduated { get; set; }
        public string? Work { get; set; }
        public string? Experience { get; set; }
        public string? HelpINeed { get; set; }
        public string? HelpIOffere { get; set; }
    }
}
