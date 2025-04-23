using System;
using System.Collections.Generic;

namespace EventManagementSystem.Models
{
    public class EventReport
    {
        public DateTime Month { get; set; }
        public string MonthName => Month.ToString("MMMM yyyy");
        public int TotalEvents { get; set; }
        public int TotalAttendees { get; set; }
        public List<EventReportItem> Events { get; set; } = new List<EventReportItem>();
    }

    public class EventReportItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime StartDate { get; set; }
        public required string Category { get; set; }
        public int? MaxAttendees { get; set; }
        public required string OrganizerName { get; set; }
    }
} 