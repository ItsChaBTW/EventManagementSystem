using System;
using System.Collections.Generic;

namespace EventManagementSystem.Models
{
    public class DashboardViewModel
    {
        // Summary statistics
        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int PastEvents { get; set; }
        public int OngoingEvents { get; set; }
        
        // Category statistics
        public List<CategoryStatistic> EventsByCategory { get; set; } = new List<CategoryStatistic>();
        
        // Event lists
        public List<EventSummary> RecentEvents { get; set; } = new List<EventSummary>();
        public List<EventSummary> UpcomingEventsList { get; set; } = new List<EventSummary>();
    }
    
    public class CategoryStatistic
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
        public string ColorCode 
        {
            get
            {
                // Generate a color based on category name for visualization
                return CategoryName switch
                {
                    "Conference" => "#4e73df", // blue
                    "Seminar" => "#1cc88a",    // green
                    "Workshop" => "#36b9cc",   // teal
                    "Party" => "#f6c23e",      // yellow
                    "Exhibition" => "#e74a3b", // red
                    "Concert" => "#fd7e14",    // orange
                    "Meeting" => "#6f42c1",    // purple
                    "Other" => "#858796",            // gray for other categories
                };
            }
        }
    }
    
    public class EventSummary
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int? MaxAttendees { get; set; }
    }
} 