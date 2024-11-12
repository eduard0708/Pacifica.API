using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacificaAPI.Dtos.Admin
{
    public class LoginResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}