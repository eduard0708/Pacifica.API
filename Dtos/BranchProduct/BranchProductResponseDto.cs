namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProductResponseDto
    {
        public int BranchId { get; set; }               // Branch ID
        public string BranchName { get; set; } = string.Empty;  // Branch name (assuming you will map this from `branchDto`)

        public int ProductId { get; set; }              // Product ID
        public string ProductName { get; set; } = string.Empty; // Product name (to map from `productDto`)

        public int ProductCategoryId { get; set; }      // Product category ID
        public string ProductCategory { get; set; } = string.Empty; // Product category name (to map from `productDto.Category`)

        public int ProductSupplierId { get; set; }      // Product supplier ID (assuming you will map this from `productDto.SupplierId`)
        public string ProductSupplier { get; set; } = string.Empty;  // Product supplier name (to map from `productDto.Supplier.Name`)

        public string? ProductSKU { get; set; }         // SKU (from `product.SKU`)

        public int StatusId { get; set; }               // Status ID (to map from `statusDto.Id`)
        public string Status { get; set; } = string.Empty;  // Status name (to map from `statusDto.Status`)

        public decimal CostPrice { get; set; }          // Cost price (from `branchProduct.CostPrice`)
        public decimal RetailPrice { get; set; }        // Retail price (from `branchProduct.RetailPrice`)
        public decimal StockQuantity { get; set; }          // Stock quantity (from `branchProduct.StockQuantity`)
        public int MinStockLevel { get; set; }          // Stock quantity (from `branchProduct.StockQuantity`)
        public int ReorderLevel { get; set; }          // Stock quantity (from `branchProduct.StockQuantity`)
        public string Remarks { get; set; } = string.Empty;  // Status name (to map from `statusDto.Status`)
        public string? CreatedBy { get; set; }          // CreatedBy (from `branchProduct.CreatedBy`)
    }
}
