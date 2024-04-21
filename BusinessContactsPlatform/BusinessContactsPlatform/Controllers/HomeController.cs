using BusinessContactsPlatform.Data;
using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.Models;
using BusinessContactsPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BusinessContactsPlatform.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context ) : base(signInManager, userManager, context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var searchUsersViewModel = new SearchUsersViewModel();
            await searchUsersViewModel.PopulateRandomUsers(_userManager, 10, User);

            
            var events = await _context.Events.ToListAsync();
            var nextWeekEvents = events
                .Where(e => e.StartDateTime >= DateTime.Now && e.StartDateTime <= DateTime.Now.AddDays(7))
                .ToList();

            var compositeViewModel = new CompositeViewModel
            {
                SearchUsersViewModel = searchUsersViewModel,
                NextWeekEvents = nextWeekEvents
            };

            return View(compositeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
