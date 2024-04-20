using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.Data.Entities
{
    public class EventUser
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }
    }
}
