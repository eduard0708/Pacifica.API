using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Admin
{
    public class LoginDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}