using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Category :AuditDetails
    {
        [Key]
        public int Id { get; set; }  // Unique identifier for the Category

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string? CategoryName { get; set; }  // Name of the category

        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }  // Optional description for the category
        public ICollection<Product>? Products { get; set; }  // One-to-many relationship with Products
    }
}
