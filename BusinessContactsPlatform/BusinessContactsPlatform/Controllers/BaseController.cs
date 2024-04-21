using BusinessContactsPlatform.Data;
using BusinessContactsPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BusinessContactsPlatform.Controllers
{
    public class BaseController : Controller
    {
        protected readonly SignInManager<AppUser> _signInManager;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly AppDbContext _context;

        public BaseController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (_signInManager.IsSignedIn(User))
            {
                var currentUser = _userManager.GetUserAsync(User).Result;

                if (currentUser != null)
                {
                    bool hasUnreadMessages = _context.ContactRequests
                        .Any(cr => cr.ReceiverUserId == currentUser.Id && !cr.IsRead);

                    ViewData["HasUnreadMessages"] = hasUnreadMessages;
                }
                else
                {
                    ViewData["HasUnreadMessages"] = false;
                }
            }
            else
            {
                ViewData["HasUnreadMessages"] = false;
            }
        }
    }
}
