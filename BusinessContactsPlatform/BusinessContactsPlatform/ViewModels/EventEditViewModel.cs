using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.ViewModels
{
    public class EventEditViewModel
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

        public string Duration { get; set; }
        public int AvailableSpace { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
