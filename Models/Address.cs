using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(100)]
        public string? CityOrMunicipality { get; set; }

        [StringLength(100)]
        public string? Barangay { get; set; }

        [StringLength(255)]
        public string? StreetAddress { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? Country { get; set; } = "Philippines";  // Default to Philippines, can be adjusted if needed

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Soft Delete: Marks when the address record was deleted (null means not deleted)
        public DateTime? DeletedAt { get; set; }

        // Tracks who created the address record (e.g., user ID or name)
        public string? CreatedBy { get; set; }

        // Indicates if the address is active (true means active)
        public bool IsActive { get; set; } = true;

        // Foreign key for the one-to-one relationship with EmployeeProfile
        public int EmployeeProfileId { get; set; }
        public virtual EmployeeProfile? EmployeeProfile { get; set; }

        // Optional: This can help with tracking when the address was last updated
        public string? UpdatedBy { get; set; }
    }
}
