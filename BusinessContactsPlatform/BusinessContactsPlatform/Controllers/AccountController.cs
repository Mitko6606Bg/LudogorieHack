using BusinessContactsPlatform.Data;
using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessContactsPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = appDbContext;

        }


        public IActionResult SearchUsers()
        {
            var model = new SearchUsersViewModel();
            model.PopulateRandomUsers(_userManager, 10 ,User);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchUsers(SearchUsersViewModel model)
        {

            if (ModelState.IsValid)
            {

                var users = await _userManager.Users
                     .Where(u => EF.Functions.Like(u.UserName, $"%{model.Username}%") && u.UserName != "admin@admin.com")
                     .ToListAsync();

                model.Users = users;

                return View(model);
            }

            return View(model);
        }

        public async Task<IActionResult> OtherProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                Id = userId,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                City = user.City,
                Graduated = user.Graduated,
                Work = user.Work,
                Experience = user.Experience,
                helpINeed = user.HelpINeed,
                helpIOffere = user.HelpIOffere,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {

                return NotFound();
            }

            var model = new AccountViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                City = user.City,
                Graduated = user.Graduated,
                Work = user.Work,
                Experience = user.Experience,
                helpINeed = user.HelpINeed,
                helpIOffere = user.HelpIOffere,
                PhoneNumber = user.PhoneNumber,
                LinkedInNames = user.LinkedInNames,
                LinkedInUsername = user.LinkedInUsername,
                LinkedInLink = user.LinkedInLink,
                InstagramNames = user.InstagramNames,
                InstagramUsername = user.InstagramUsername,
                InstagramLink = user.InstagramLink,
                FacebookNames = user.FacebookNames,
                FacebookUsername = user.FacebookUsername,
                FacebookLink = user.FacebookLink
            };

            return View(model);
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (User.IsInRole("Member"))
                    {
                        var currentUser = await _userManager.GetUserAsync(User);

                        
                        if (string.IsNullOrEmpty(currentUser.LinkedInNames))
                        {
                            return RedirectToAction(nameof(AddSocialMedia));
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Name = model.Name,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    City = model.City,
                    Graduated = model.Graduated,
                    Work = model.Work,
                    Experience = model.Experience,
                    HelpINeed = model.helpINeed,
                    HelpIOffere = model.helpIOffere,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");
                   
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            Logout();
            return View(model);
        }

        public IActionResult AddSocialMedia()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSocialMedia(AddSocialMadiaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    return NotFound();
                }

                currentUser.LinkedInNames = model.LinkedInNames;
                currentUser.LinkedInUsername = model.LinkedInUsername;
                currentUser.LinkedInLink = model.LinkedInLink;
                currentUser.InstagramNames = model.InstagramNames;
                currentUser.InstagramUsername = model.InstagramUsername;
                currentUser.InstagramLink = model.InstagramLink;
                currentUser.FacebookNames = model.FacebookNames;
                currentUser.FacebookUsername = model.FacebookUsername;
                currentUser.FacebookLink = model.FacebookLink;

                var result = await _userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                City = user.City,
                Graduated = user.Graduated,
                Work = user.Work,
                Experience = user.Experience,
                helpINeed = user.HelpINeed,
                helpIOffere = user.HelpIOffere,
                PhoneNumber = user.PhoneNumber,
                LinkedInNames = user.LinkedInNames,
                LinkedInUsername = user.LinkedInUsername,
                LinkedInLink = user.LinkedInLink,
                InstagramNames = user.InstagramNames,
                InstagramUsername = user.InstagramUsername,
                InstagramLink = user.InstagramLink,
                FacebookNames = user.FacebookNames,
                FacebookUsername = user.FacebookUsername,
                FacebookLink = user.FacebookLink
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Update user profile information
            user.Name = model.Name;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.City = model.City;
            user.Graduated = model.Graduated;
            user.Work = model.Work;
            user.Experience = model.Experience;
            user.HelpINeed = model.helpINeed;
            user.HelpIOffere = model.helpIOffere;
            user.PhoneNumber = model.PhoneNumber;

            
            user.LinkedInNames = model.LinkedInNames;
            user.LinkedInUsername = model.LinkedInUsername;
            user.LinkedInLink = model.LinkedInLink;
            user.InstagramNames = model.InstagramNames;
            user.InstagramUsername = model.InstagramUsername;
            user.InstagramLink = model.InstagramLink;
            user.FacebookNames = model.FacebookNames;
            user.FacebookUsername = model.FacebookUsername;
            user.FacebookLink = model.FacebookLink;
            

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContactRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);


            var unreadContactRequests = await _context.ContactRequests
                .Where(cr => cr.ReceiverUserId == currentUser.Id && cr.Status == ContactRequestStatus.Pending)
                .ToListAsync();

            
            foreach (var request in unreadContactRequests)
            {
                request.IsRead = true;
            }

            await _context.SaveChangesAsync(); 

            
            var contactRequestViewModels = new List<ContactRequestViewModel>();
            foreach (var request in unreadContactRequests)
            {
                var senderUser = await _userManager.FindByIdAsync(request.SenderUserId);
                if (senderUser != null)
                {
                    var viewModel = new ContactRequestViewModel
                    {
                        Id = request.Id,
                        SenderUserId = request.SenderUserId,
                        SenderUsername = senderUser.UserName
                    };
                    contactRequestViewModels.Add(viewModel);
                }
            }

            
            return View(contactRequestViewModels);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var contactRequestsSent = await _context.ContactRequests
                .Where(cr => cr.SenderUserId == currentUser.Id && cr.Status == ContactRequestStatus.Accepted)
                .ToListAsync();

            var contactRequestsReceived = await _context.ContactRequests
                .Where(cr => cr.ReceiverUserId == currentUser.Id && cr.Status == ContactRequestStatus.Accepted)
                .ToListAsync();

            var allAcceptedContactRequests = contactRequestsSent.Concat(contactRequestsReceived).ToList();

            var userIds = allAcceptedContactRequests.SelectMany(cr =>
                cr.SenderUserId == currentUser.Id ? new[] { cr.ReceiverUserId } : new[] { cr.SenderUserId })
                .ToList();

            var usersWithAcceptedRequests = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var viewModel = new MyContactsViewModel
            {
                Users = usersWithAcceptedRequests
            };

            return View(viewModel);
        }


    }
}
