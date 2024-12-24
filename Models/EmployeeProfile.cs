using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models.EmployeManagement
{
    public class EmployeeProfile : AuditDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]  // Ensures that EmployeeId matches IdentityUser.Id length
        public string? EmployeeId { get; set; }  // Foreign key to Employee

        public virtual Employee? Employee { get; set; }  // Navigation property to Employee

        // Address-related fields
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
        public string Country { get; set; } = "Philippines";  // Default to Philippines

    }
}
