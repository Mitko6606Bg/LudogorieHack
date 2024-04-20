using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BusinessContactsPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> userManager;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            this.userManager = userManager;

        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
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
                PhoneNumber = user.PhoneNumber
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
                        return RedirectToAction("Index", "Home");
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

                var result = await userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await userManager.AddToRoleAsync(user, "Member");
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

        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);
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
                PhoneNumber = user.PhoneNumber
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

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
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

            var result = await userManager.UpdateAsync(user);
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
    }
}
