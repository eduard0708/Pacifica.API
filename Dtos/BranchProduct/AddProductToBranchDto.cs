using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.BranchProduct
{
    public class AddProductToBranchDto
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int StatusId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "CostPrice must be greater than zero.")]
        public decimal CostPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "RetailPrice must be greater than zero.")]
        public decimal RetailPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ReorderLevel must be greater than zero.")]
        public int ReorderLevel { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "MinStockLevel must be greater than zero.")]
        public int MinStockLevel { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
    }
}
