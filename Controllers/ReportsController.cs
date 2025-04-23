using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reports
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reports/Monthly
        public async Task<IActionResult> Monthly(int? month, int? year)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            // If month/year not provided, use current month
            var now = DateTime.Now;
            month = month ?? now.Month;
            year = year ?? now.Year;

            // Create a date for the first day of the specified month
            var startDate = new DateTime(year.Value, month.Value, 1);
            // Get the first day of the next month
            var endDate = startDate.AddMonths(1);

            // Query events for the month
            var events = await _context.Events
                .Include(e => e.Organizer)
                .Where(e => e.OrganizerId == currentUser.Id) // Only user's events
                .Where(e => e.StartDate >= startDate && e.StartDate < endDate)
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            // Create the report
            var report = new EventReport
            {
                Month = startDate,
                TotalEvents = events.Count,
                TotalAttendees = events.Sum(e => e.MaxAttendees ?? 0),
                Events = events.Select(e => new EventReportItem
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDate = e.StartDate,
                    Category = e.Category,
                    MaxAttendees = e.MaxAttendees,
                    OrganizerName = e.Organizer?.FullName ?? "Unknown"
                }).ToList()
            };

            // Add ViewBag data for the month selector
            ViewBag.CurrentMonth = month;
            ViewBag.CurrentYear = year;
            ViewBag.MonthsList = Enumerable.Range(1, 12).Select(m => new { Value = m, Text = new DateTime(2000, m, 1).ToString("MMMM") });
            ViewBag.YearsList = Enumerable.Range(year.Value - 5, 10).Select(y => new { Value = y, Text = y.ToString() });

            return View(report);
        }
    }
} 