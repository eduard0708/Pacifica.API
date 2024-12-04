using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class AuditDetails
    {

        [StringLength(1500)] 
        [Required(ErrorMessage = "Max Remarks Character is only 1500.")]
        public string? Remarks { get; set; }

        [Required(ErrorMessage = "Deleted status is required.")]
        public bool? IsDeleted { get; set; } = false;

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