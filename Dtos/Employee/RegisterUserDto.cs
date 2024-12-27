using System.ComponentModel.DataAnnotations;
namespace Pacifica.API.Dtos.Employee
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "EmployeeId is required.")]
        [StringLength(450, ErrorMessage = "EmployeeId cannot be longer than 450 characters.")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]            
        [StringLength(25)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? FirstName { get; set; }
        [StringLength(20)]
        public string? LastName { get; set; }
    }
}