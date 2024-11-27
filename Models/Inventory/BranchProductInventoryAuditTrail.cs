

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{
   public class BranchProductInventoryAuditTrail
{
   [Key]
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int ProductId { get; set; }

    public int PreviousStockQuantity { get; set; }
    public int AdjustedQuantity { get; set; }
    public string? AdjustedBy { get; set; }
    public DateTime DateAdjusted { get; set; }
    public string? AdjustmentReason { get; set; }

    // Navigation property to BranchProduct
    public BranchProduct? BranchProduct { get; set; }
}

}