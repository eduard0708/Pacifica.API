using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        // Foreign Key for User
        public string? EmployeeId { get; set; }  // This will reference the User's Id

        // Add any additional profile information here
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        // Navigation property to the User
        public User? User { get; set; }
    }
}