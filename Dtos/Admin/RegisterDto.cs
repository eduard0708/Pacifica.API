using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Admin
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public string? EmployeeId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        // [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
        // public string? Position { get; set; }

        // [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        // public string? Department { get; set; }
    }
}
