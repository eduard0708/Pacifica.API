using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Employee
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "EmployeeId is required.")]
        [StringLength(450, ErrorMessage = "EmployeeId cannot be longer than 128 characters.")]
        public string EmployeeId { get; set; } = string.Empty;

        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public List<string>? Roles { get; set; }
        public List<int>? BranchIds  { get; set; }
        public string? Password { get; set; }
        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }
    }
}