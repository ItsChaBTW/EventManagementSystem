using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;

namespace EventManagementSystem.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var events = await _context.Events
                .Include(e => e.Organizer)
                .Where(e => e.OrganizerId == currentUser.Id)
                .ToListAsync();
            return View(events);
        }

        

        // GET: Events/DetailsPartial/5
        public async Task<IActionResult> DetailsPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var @event = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id && m.OrganizerId == currentUser.Id);
            
            if (@event == null)
            {
                return NotFound();
            }

            return PartialView("_EventDetailsPartial", @event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,StartDate,EndDate,Location,Category,MaxAttendees")] Event @event)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                @event.OrganizerId = user.Id;
                
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id && m.OrganizerId == currentUser.Id);
                
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,Location,Category,MaxAttendees")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            // Ensure the current user is the owner of the event
            var currentUser = await _userManager.GetUserAsync(User);
            var existingEvent = await _context.Events.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == currentUser.Id);
                
            if (existingEvent == null)
            {
                return NotFound();
            }
            
            // Preserve the OrganizerId
            @event.OrganizerId = currentUser.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var @event = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id && m.OrganizerId == currentUser.Id);
                
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id && m.OrganizerId == currentUser.Id);
                
            if (@event == null)
            {
                return NotFound();
            }
            
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        
    }
} 