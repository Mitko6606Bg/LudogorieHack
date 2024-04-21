using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessContactsPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;

namespace BusinessContactsPlatform.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EventController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View(_context.Events.ToList());
        }

        [Authorize]
        public async Task<IActionResult> MyEvents()
        {
            var user = await _userManager.GetUserAsync(User);

            var myEvents = await _context.Events
                .Where(e => e.Organizer == user.Id)
                .ToListAsync();

            return View(myEvents);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.EventUsers)
                .ThenInclude(eu => eu.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var imageBase64 = Convert.ToBase64String(@event.Image);
            var imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64);
            ViewBag.ImageDataURL = imageDataURL;

            return View(@event);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                TimeSpan duration = model.EndDateTime - model.StartDateTime;
                string durationString = $"{(int)duration.TotalDays} days and {(int)duration.Hours} hours";

                var @event = new Event
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    Location = model.Location,
                    OnlineLink = model.OnlineLink,
                    Organizer = user.Id,
                    Category = model.Category,
                    Price = model.Price,
                    AvailableSpace = model.AvailableSpace,
                    Image = imageData,
                    Duration = durationString
                };

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }


            var model = new EventEditViewModel
            {
                Title = @event.Title,
                Description = @event.Description,
                StartDateTime = @event.StartDateTime,
                EndDateTime = @event.EndDateTime,
                Location = @event.Location,
                OnlineLink = @event.OnlineLink,
                Category = @event.Category,
                Price = @event.Price,
                AvailableSpace = @event.AvailableSpace,
                Image = @event.Image,
                Duration = @event.Duration,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }

                TimeSpan duration = model.EndDateTime - model.StartDateTime;
                string durationString = $"{(int)duration.TotalDays} days and {(int)duration.Hours} hours";

                @event.Title = model.Title;
                @event.Description = model.Description;
                @event.StartDateTime = model.StartDateTime;
                @event.EndDateTime = model.EndDateTime;
                @event.Location = model.Location;
                @event.OnlineLink = model.OnlineLink;
                @event.Category = model.Category;
                @event.Price = model.Price;
                @event.AvailableSpace = model.AvailableSpace;
                @event.Image = model.Image;
                @event.Duration = durationString;

                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _context.Events
                .FirstOrDefault(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var @event = _context.Events.Find(id);
            _context.Events.Remove(@event);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Join(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            
            var alreadyJoined = await _context.UserEvents
                .AnyAsync(eu => eu.EventId == id && eu.UserId == user.Id);

            if (alreadyJoined)
            {
                TempData["Message"] = "You are already joined this event.";
                return RedirectToAction("Details", new { id = id });
            }

            
            if (@event.AvailableSpace <= 0)
            {
                TempData["Message"] = "Sorry, there is no available space for this event.";
                return RedirectToAction("Details", new { id = id });
            }

            
            @event.AvailableSpace -= 1;

            var eventUser = new EventUser
            {
                EventId = id.Value,
                UserId = user.Id
            };

            _context.UserEvents.Add(eventUser);
            await _context.SaveChangesAsync();

            TempData["Message"] = "You have successfully joined the event.";

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var eventUser = await _context.UserEvents
                .FirstOrDefaultAsync(eu => eu.EventId == id && eu.UserId == userId);

            if (eventUser == null)
            {
                TempData["Message"] = "You are not joined this event.";
                return RedirectToAction("Details", new { id = id });
            }

            _context.UserEvents.Remove(eventUser);

            @event.AvailableSpace++;

            await _context.SaveChangesAsync();

            TempData["Message"] = "You have successfully left the event.";

            return RedirectToAction("Details", new { id = id });
        }


    }
}
