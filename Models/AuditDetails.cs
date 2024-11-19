using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class AuditDetails
    {

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        

        public DateTime? UpdatedAt { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; } 

        [StringLength(100)]
        public string? DeletedBy { get; set; }

    }
}