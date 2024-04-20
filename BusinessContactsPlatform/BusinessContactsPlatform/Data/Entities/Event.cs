namespace BusinessContactsPlatform.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string OnlineLink { get; set; }
        public string Organizer { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public ICollection<EventUser> EventUsers { get; set; }
    }
}
