using Microsoft.AspNetCore.Identity;
using Pacifica.API.Models.EmployeManagement;
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Employee : IdentityUser 
    {
        // EmployeeId must be the key', 
        [Key]
        [Required]
        [StringLength(128)]  // Maximum length for EmployeeId, set by your system design
        public string EmployeeId { get; set; } = string.Empty;

        // One-to-One relationship with EmployeeProfile
        public int? EmployeeProfileId { get; set; }
        public virtual EmployeeProfile? EmployeeProfile { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(100)] // Set max length for Department
        public virtual Department? Department { get; set; }

        public int? PositionId { get; set; }
        [StringLength(100)] // Set max length for Department
        public virtual Position? Position { get; set; }


        // Optional roles, if needed in this model (IdentityUser already includes role management)
        public virtual ICollection<IdentityRole>? Roles { get; set; }
        public ICollection<EmployeeBranch>? EmployeeBranches { get; set; }

        // Audit fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Soft Delete: Marks when the employee record was deleted (null means not deleted)
        public DateTime? DeletedAt { get; set; }

        // Tracks who created the employee record
        [StringLength(100)] // Maximum length for CreatedBy field
        public string? CreatedBy { get; set; }

        // Indicates if the employee is active (true means active)
        [Required] // Make IsActive a required field
        public bool IsActive { get; set; } = true;

        // Optional: This can help with tracking when the employee was last updated
        [StringLength(100)] // Maximum length for UpdatedBy field
        public string? UpdatedBy { get; set; }
    }
}
