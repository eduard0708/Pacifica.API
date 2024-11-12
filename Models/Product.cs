namespace Pacifica.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ReorderLevel { get; set; }
        public int MinStockLevel { get; set; }
        public string ProductStatus { get; set; } = string.Empty;
        public int CreatedBy { get; set; }

        // Navigation property
        public int CategoryId { get; set; }  // Foreign key to Category
        public Category? Category { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<StockTransactionInOut>? StockTransactionInOuts { get; set; }
    }
}

