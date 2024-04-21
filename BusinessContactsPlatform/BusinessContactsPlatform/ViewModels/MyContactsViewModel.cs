using BusinessContactsPlatform.Data.Entities;

namespace BusinessContactsPlatform.ViewModels
{
    public class MyContactsViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<AppUser> Users { get; set; }

    }
}
