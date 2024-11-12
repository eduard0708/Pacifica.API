using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class User : IdentityUser
    {
        [Key] // Mark EmployeeId as the primary key
        public string? EmployeeId { get; set; }  // EmployeeId will be the primary key

        // Other custom properties
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Navigation property for UserProfile
        public UserProfile? UserProfile { get; set; }
    }
}
