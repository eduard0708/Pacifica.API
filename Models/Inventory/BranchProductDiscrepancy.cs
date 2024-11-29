
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{
    public class BranchProductDiscrepancy : AuditDetails
    {
        public int Id { get; set; }  // Primary Key
        public int BranchId { get; set; }  // Reference to the specific branch-product
        public int ProductId { get; set; }  // Reference to the specific branch-product
        public DateTime DateReported { get; set; }  // Date of the inventory
        public Months Month { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "decimal(7,1)")]
        public Decimal ActualQuantity { get; set; }  // Actual quantity counted
        
        [Column(TypeName = "decimal(7,1)")]
        public Decimal SystemQuantity { get; set; }  // Quantity from the system
        
        [Column(TypeName = "decimal(7,1)")]
        public Decimal Discrepancy { get; set; }  // Difference between actual and system quantity
        
        [Column(TypeName = "decimal(18,2)")]
        public Decimal DiscrepancyValue { get; set; }  // Total value of the discrepancy
        
        public BranchProduct? BranchProduct { get; set; }
    }

}