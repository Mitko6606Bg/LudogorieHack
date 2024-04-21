using BusinessContactsPlatform.Data.Entities;

namespace BusinessContactsPlatform.ViewModels
{
    public class CompositeViewModel
    {
        public SearchUsersViewModel SearchUsersViewModel { get; set; }
        public List<Event> NextWeekEvents { get; set; }
    }
}
