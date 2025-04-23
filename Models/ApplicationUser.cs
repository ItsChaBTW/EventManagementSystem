using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public required string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Organization")]
        public required string Organization { get; set; } = string.Empty;
    }
} 