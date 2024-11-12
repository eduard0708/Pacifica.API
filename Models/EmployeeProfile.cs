using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models
{
    public class EmployeeProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary Key

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

        [StringLength(15)]
        [Phone]
        public string? PhoneNumber { get; set; }  // Employee's phone number

        [StringLength(20)]
        public string? Gender { get; set; }  // Employee's gender

        [StringLength(50)]
        public string? EmploymentStatus { get; set; }  // Employment status (Full-Time, Part-Time, etc.)

        public string? EmployeeId { get; set; }  // Foreign key to IdentityUser (Employee)
        public virtual Employee? Employee { get; set; }  // Navigation to Employee entity

        [Required]
        public int AddressId { get; set; }  // Foreign key to Address
        public virtual Address? Address { get; set; }  // Navigation to Address entity

        public DateTime? DeletedAt { get; set; }  // Soft delete: Marks when the profile was deleted

        [StringLength(100)]
        public string? CreatedBy { get; set; }  // Tracks who created the record

        [Required]
        public bool IsActive { get; set; } = true;  // Indicates if the profile is active

        [StringLength(100)]
        public string? UpdatedBy { get; set; }  // Tracks who last updated the profile

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date the profile was created

        public DateTime? UpdatedAt { get; set; }  // Date the profile was last updated
    }
}
