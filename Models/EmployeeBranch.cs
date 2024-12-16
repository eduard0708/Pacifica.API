using System;
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class EmployeeBranch
    {
        // Composite Key: EmployeeId and BranchId
        [Key]
        public string? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Key]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        // Additional properties related to the relationship
        [Required]
        [StringLength(100)]
        public string? Role { get; set; }  // Role/Position of the employee at the branch (e.g., Manager, Staff)

        // Audit fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Tracks who created the employee-branch relationship record
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        // Tracks who last updated the employee-branch relationship record
        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        // Is the relationship active
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
