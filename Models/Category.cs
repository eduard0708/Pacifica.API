using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }  // Unique identifier for the Category

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string? CategoryName { get; set; }  // Name of the category

        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }  // Optional description for the category

        public ICollection<Product>? Products { get; set; }  // One-to-many relationship with Products

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the category was created

        public DateTime? UpdatedAt { get; set; }  // Date when the category was last updated

        [StringLength(100, ErrorMessage = "Creator's name cannot exceed 100 characters.")]
        public string? CreatedBy { get; set; }  // Tracks who created the category

        [StringLength(100, ErrorMessage = "Updator's name cannot exceed 100 characters.")]
        public string? UpdatedBy { get; set; }  // Tracks who last updated the category

        [Required]
        public bool IsActive { get; set; } = true;  // Indicates if the category is active

        public DateTime? DeletedAt { get; set; }  // Soft delete: Tracks when the category was deleted
    }
}
