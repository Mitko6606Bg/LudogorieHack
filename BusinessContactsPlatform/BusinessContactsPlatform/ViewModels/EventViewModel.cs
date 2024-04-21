using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public string Location { get; set; }

        public string OnlineLink { get; set; }

        public string Category { get; set; }

        [Display(Name = "Available Space")]
        [Required(ErrorMessage = "Please enter the available space.")]
        public int AvailableSpace {  get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
