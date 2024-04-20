using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessContactsPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;

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
                    Price = model.Price
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

            var model = new EventViewModel
            {
                Title = @event.Title,
                Description = @event.Description,
                StartDateTime = @event.StartDateTime,
                EndDateTime = @event.EndDateTime,
                Location = @event.Location,
                OnlineLink = @event.OnlineLink,
                Category = @event.Category,
                Price = @event.Price
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
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

                @event.Title = model.Title;
                @event.Description = model.Description;
                @event.StartDateTime = model.StartDateTime;
                @event.EndDateTime = model.EndDateTime;
                @event.Location = model.Location;
                @event.OnlineLink = model.OnlineLink;
                @event.Category = model.Category;
                @event.Price = model.Price;

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

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.Id == id);
        //}

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

            // Check if the user is already joined the event
            var alreadyJoined = await _context.UserEvents
                .AnyAsync(eu => eu.EventId == id && eu.UserId == user.Id);

            if (alreadyJoined)
            {
                TempData["Message"] = "You are already joined this event.";
                return RedirectToAction("Details", new { id = id });
            }

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
    }
}
