using System.ComponentModel.DataAnnotations;

namespace BusinessContactsPlatform.Data.Entities
{
    public class ContactRequest
    {
        [Key]
        public int Id { get; set; }

        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public bool IsRead { get; set; }
        public ContactRequestStatus Status { get; set; }
    }

    public enum ContactRequestStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
