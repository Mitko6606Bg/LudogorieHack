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
        public string? LinkedInNames { get; set; }
        public string? LinkedInUsername { get; set; }
        public string? LinkedInLink { get; set; }
        public string? InstagramNames { get; set; }
        public string? InstagramUsername { get; set; }
        public string? InstagramLink { get; set; }
        public string? FacebookNames { get; set; }
        public string? FacebookUsername { get; set; }
        public string? FacebookLink { get; set; }

    }
}
