using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class AuditDetails
    {
        
        [StringLength(1500)]  // Limits the length of the SKU
        public string? Remarks { get; set; }

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