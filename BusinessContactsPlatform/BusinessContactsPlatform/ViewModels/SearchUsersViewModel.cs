using BusinessContactsPlatform.Data;
using BusinessContactsPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BusinessContactsPlatform.ViewModels
{
    public class SearchUsersViewModel
    {
        public string Username { get; set; }
        public List<AppUser> Users { get; set; }
        public List<AppUser> RandomUsers { get; set; }

        public SearchUsersViewModel()
        {
            Users = new List<AppUser>();
            RandomUsers = new List<AppUser>();
        }


        public async Task PopulateRandomUsers(UserManager<AppUser> userManager, int count, ClaimsPrincipal userPrincipal)
        {
            var currentUser = await userManager.GetUserAsync(userPrincipal);

            if (currentUser != null)
            {
                var users = await userManager.Users
                    .Where(u => u.UserName != "admin@admin.com" && u.Id != currentUser.Id)
                    .OrderBy(u => Guid.NewGuid())
                    .Take(count)
                    .ToListAsync();

                RandomUsers = users;
            }
            else
            {
                var users = await userManager.Users
                    .Where(u => u.UserName != "admin@admin.com")
                    .OrderBy(u => Guid.NewGuid())
                    .Take(count)
                    .ToListAsync();

                RandomUsers = users;
            }
        }


    }
}
