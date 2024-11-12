using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models
{
    public class EmployeeProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]  // Ensures FirstName is provided
        [StringLength(100)]  // Limits the length of the FirstName to 100 characters
        public string? FirstName { get; set; }

        [StringLength(100)]  // Limits the length of the MiddleName to 100 characters
        public string? MiddleName { get; set; }

        [Required]  // Ensures LastName is provided
        [StringLength(100)]  // Limits the length of the LastName to 100 characters
        public string? LastName { get; set; }

        [StringLength(50)]  // Limits the length of the Position to 50 characters
        public string? Position { get; set; }

        [StringLength(50)]  // Limits the length of the Department to 50 characters
        public string? Department { get; set; }

        // Date validation for DateOfBirth (optional: can be adjusted to a specific age range if needed)
        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfHire { get; set; }

        [StringLength(15)]  // Limits the PhoneNumber to 15 characters
        [Phone]  // Ensures the PhoneNumber is in a valid phone number format
        public string? PhoneNumber { get; set; }

        [EmailAddress]  // Ensures the Email is in a valid email format
        [StringLength(100)]  // Limits the Email length to 100 characters
        public string? Email { get; set; }

        [StringLength(20)]  // Limits the Gender to 20 characters (this is optional and can be adjusted)
        public string? Gender { get; set; }

        [StringLength(50)]  // Limits the EmploymentStatus to 50 characters
        public string? EmploymentStatus { get; set; }  // e.g., Full-Time, Part-Time, Contractual

        // Foreign key to Employee (One-to-One Relationship)
        public string? EmployeeId { get; set; } // Foreign key to IdentityUser (Employee)
        public virtual Employee? Employee { get; set; }

        // One-to-One relationship with Address
        [Required]  // Ensures AddressId is provided
        public int AddressId { get; set; }
        public virtual Address? Address { get; set; }

        // Soft Delete: Marks when the employee profile record was deleted (null means not deleted)
        public DateTime? DeletedAt { get; set; }

        // Tracks who created the employee profile record (e.g., user ID or name)
        [StringLength(100)]  // Limits the CreatedBy field length to 100 characters
        public string? CreatedBy { get; set; }

        // Indicates if the employee profile is active (true means active)
        [Required]  // Ensures IsActive is always provided
        public bool IsActive { get; set; } = true;

        // Optional: This can help with tracking when the employee profile was last updated
        [StringLength(100)]  // Limits the UpdatedBy field length to 100 characters
        public string? UpdatedBy { get; set; }

        // Date fields for tracking creation and updates
        [Required]  // Ensures CreatedAt has a value
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
