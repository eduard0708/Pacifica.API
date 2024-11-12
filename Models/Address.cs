using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary Key

        [StringLength(100)]
        public string? Region { get; set; }  // Region (e.g., Luzon, Visayas)

        [StringLength(100)]
        public string? Province { get; set; }  // Province or state

        [StringLength(100)]
        public string? CityOrMunicipality { get; set; }  // City or Municipality

        [StringLength(100)]
        public string? Barangay { get; set; }  // Barangay (village/subdivision)

        [StringLength(255)]
        public string? StreetAddress { get; set; }  // Street or building address

        [StringLength(10)]
        public string? PostalCode { get; set; }  // Postal Code

        [StringLength(50)]
        public string? Country { get; set; } = "Philippines";  // Default to Philippines

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the address was created

        public DateTime? UpdatedAt { get; set; }  // Date when the address was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete timestamp (null means active)

        public string? CreatedBy { get; set; }  // Tracks who created the address record

        public bool IsActive { get; set; } = true;  // Indicates if the address is active

        // Foreign key linking to EmployeeProfile
        public int EmployeeProfileId { get; set; }
        public virtual EmployeeProfile? EmployeeProfile { get; set; }  // Navigation property

        public string? UpdatedBy { get; set; }  // Tracks who last updated the address
    }
}
