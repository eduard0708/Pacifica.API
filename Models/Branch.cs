using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }  // Primary key for the Branch entity

        [Required(ErrorMessage = "Branch name is required.")]
        [MaxLength(255, ErrorMessage = "Branch name cannot exceed 255 characters.")]
        public string BranchName { get; set; } = string.Empty;  // Name of the branch

        [Required(ErrorMessage = "Branch location is required.")]
        [MaxLength(500, ErrorMessage = "Branch location cannot exceed 500 characters.")]
        public string BranchLocation { get; set; } = string.Empty;  // Location of the branch

        public ICollection<EmployeeBranch>? EmployeeBranches { get; set; }  // Many-to-many relation with Employee

        public ICollection<BranchProduct>? BranchProducts { get; set; }  // Many-to-many relation with Product

        public ICollection<StockInOut>? StockInOuts { get; set; }  // Many-to-many relation with Product

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the branch was created

        public DateTime? UpdatedAt { get; set; }  // Date when the branch was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete date (null means not deleted)

        [StringLength(100, ErrorMessage = "Creator's name cannot exceed 100 characters.")]
        public string? CreatedBy { get; set; }  // Tracks who created the branch

        [Required(ErrorMessage = "Branch active status is required.")]
        public bool IsActive { get; set; } = true;  // Indicates if the branch is active

        [StringLength(100, ErrorMessage = "Updator's name cannot exceed 100 characters.")]
        public string? UpdatedBy { get; set; }  // Tracks who last updated the branch
    }
}
