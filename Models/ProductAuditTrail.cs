using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class ProductAuditTrail
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        
        [Required]
        public string Action { get; set; } = string.Empty; // "Created", "Updated", "Deleted", or "Restored"

        [StringLength(255)]
        public string? OldValue { get; set; } // JSON or string representation of old data

        [StringLength(255)]
        public string? NewValue { get; set; } // JSON or string representation of new data

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? ActionBy { get; set; } // User who performed the action

        public Product? Product { get; set; }


    }
}
