using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.InventoryNormalization
{
    public class CreateInventoryNormalizationDto
    { 
        [Required]
        public int InventoryId { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Adjusted Quantity must be a positive number.")]
        public decimal SystemQuantity { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "System Quantity must be a positive number.")]
        public decimal ActualQuantity { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Actual Quantity  must be a positive number.")]
        public decimal CostPrice { get; set; }
        [Required]
        public DateTime NormalizationDate { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
        
    }
}