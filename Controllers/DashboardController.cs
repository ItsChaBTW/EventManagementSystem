using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            // Dashboard metrics
            var today = DateTime.Today;
            var allEvents = await _context.Events
                .Where(e => e.OrganizerId == currentUser.Id)
                .ToListAsync();
                
            var dashboardVM = new DashboardViewModel
            {
                TotalEvents = allEvents.Count,
                UpcomingEvents = allEvents.Count(e => e.StartDate > today),
                PastEvents = allEvents.Count(e => e.EndDate < today),
                OngoingEvents = allEvents.Count(e => e.StartDate <= today && e.EndDate >= today),
                
                // Event categories
                EventsByCategory = allEvents
                    .GroupBy(e => e.Category)
                    .Select(g => new CategoryStatistic
                    {
                        CategoryName = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(c => c.Count)
                    .ToList(),
                
                // Recent events
                RecentEvents = allEvents
                    .OrderByDescending(e => e.StartDate)
                    .Take(5)
                    .Select(e => new EventSummary
                    {
                        Id = e.Id,
                        Title = e.Title,
                        StartDate = e.StartDate,
                        Location = e.Location,
                        Category = e.Category,
                        MaxAttendees = e.MaxAttendees
                    })
                    .ToList(),
                
                // Upcoming events
                UpcomingEventsList = allEvents
                    .Where(e => e.StartDate > today)
                    .OrderBy(e => e.StartDate)
                    .Take(5)
                    .Select(e => new EventSummary
                    {
                        Id = e.Id,
                        Title = e.Title,
                        StartDate = e.StartDate,
                        Location = e.Location,
                        Category = e.Category,
                        MaxAttendees = e.MaxAttendees
                    })
                    .ToList()
            };
            
            return View(dashboardVM);
        }
    }
} 