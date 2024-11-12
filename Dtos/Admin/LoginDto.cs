using System.ComponentModel.DataAnnotations;

namespace PacificaAPI.Dtos.Admin
{
    public class LoginDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}