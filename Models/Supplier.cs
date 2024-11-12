using System;
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Supplier
    {
        public int Id { get; set; }  // Unique identifier for the supplier

        public string? SupplierName { get; set; }  // Name of the supplier

        public string? ContactPerson { get; set; }  // Name of the contact person at the supplier

        public string? ContactNumber { get; set; }  // Contact number for the supplier

        // Audit fields
        [Required]  
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date the supplier was created

        public DateTime? UpdatedAt { get; set; }  // Date the supplier was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete timestamp (null means not deleted)

        [StringLength(100)]  
        public string? CreatedBy { get; set; }  // User who created the supplier record

        [Required]  
        public bool IsActive { get; set; } = true;  // Indicates whether the supplier is active

        [StringLength(100)]  
        public string? UpdatedBy { get; set; }  // User who last updated the supplier record
    }
}
