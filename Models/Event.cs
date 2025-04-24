using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Event Name")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        [Display(Name = "Attendee Count")]
        public int? MaxAttendees { get; set; }
         
        // Navigation property for organizer
        public string? OrganizerId { get; set; }
        
        // Navigation property
        public ApplicationUser? Organizer { get; set; }
    }
} 