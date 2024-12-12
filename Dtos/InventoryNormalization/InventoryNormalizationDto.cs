using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.InventoryNormalization
{
    public class InventoryNormalizationDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Adjusted Quantity must be a positive number.")]
        public decimal AdjustedQuantity { get; set; }
        public decimal DescripancyValue { get; set; }
        
        [Required]
        public DateTime NormalizationDate { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
    }
}