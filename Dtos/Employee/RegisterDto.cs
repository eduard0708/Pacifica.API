using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Employee
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }
    }
}