using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.Data.Entities
{
    public class SocialMedia
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string? LinkedInNames { get; set; }

        [Required]
        public string? LinkedInUsername { get; set; }

        [Required]
        public string? LinkedInLink { get; set; }

        [Required]
        public string? InstagramNames { get; set; }

        [Required]
        public string? InstagramUsername { get; set; }

        [Required]
        public string? InstagramLink { get; set; }

        [Required]
        public string? FacebookNames { get; set; }

        [Required]
        public string? FacebookUsername { get; set; }

        [Required]
        public string? FacebookLink { get; set; }
    }
}
