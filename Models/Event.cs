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
        [FutureDate(ErrorMessage = "Start date must be in the future for new events")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Display(Name = "End Date")]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after the start date")]
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

    // Custom validation attribute to ensure the date is in the future
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // For edit actions, we don't want to enforce future date
            if (validationContext.ObjectInstance is Event evt && evt.Id > 0)
            {
                return ValidationResult.Success;
            }

            DateTime date = (DateTime)value;
            if (date < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            
            return ValidationResult.Success;
        }
    }

    // Custom validation attribute to compare two dates
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime endDate = (DateTime)value;
            
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
                
            var startDate = (DateTime)property.GetValue(validationContext.ObjectInstance);
            
            if (endDate <= startDate)
            {
                return new ValidationResult(ErrorMessage);
            }
            
            return ValidationResult.Success;
        }
    }
} 