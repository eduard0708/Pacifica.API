using Microsoft.AspNetCore.Identity;
using Pacifica.API.Models.EmployeManagement;
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Employee : IdentityUser
    {
        [Key]
        [Required]
        [StringLength(128)]  // Maximum length for EmployeeId, set by your system design
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }  // Employee's first name

        [StringLength(100)]
        public string? MiddleName { get; set; }  // Employee's middle name

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }  // Employee's last name

        public DateTime? DateOfBirth { get; set; }  // Employee's date of birth

        public DateTime? DateOfHire { get; set; }  // Employee's date of hire

        [StringLength(20)]
        public string? Gender { get; set; }  // Employee's gender

        [StringLength(50)]
        public string? EmploymentStatus { get; set; }  // Employment status (Full-Time, Part-Time, etc.)

        // One-to-One relationship with EmployeeProfile (which includes address)
        public int EmployeeProfileId { get; set; }
        public virtual EmployeeProfile? EmployeeProfile { get; set; }

        // Department and Position (referencing other entities)
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public int? PositionId { get; set; }
        public virtual Position? Position { get; set; }

        // Roles, branches, and other employee-related properties
        public virtual ICollection<IdentityRole>? Roles { get; set; }
        public ICollection<EmployeeBranch>? EmployeeBranches { get; set; }

        // Indicates if the employee is active
        [Required]
        public bool IsActive { get; set; } = true;

        [StringLength(1500, ErrorMessage = "Max Remarks Character is only 1500.")]
        public string? Remarks { get; set; }

        [Required(ErrorMessage = "Deleted status is required.")]
        public bool IsDeleted { get; set; } = false;

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
