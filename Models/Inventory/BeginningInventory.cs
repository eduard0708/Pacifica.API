using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{

public class BeginningInventory
{
    [Key]
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int ProductId { get; set; }

    [Required]
    public int InitialStockQuantity { get; set; }
    public decimal InitialCostPrice { get; set; }
    public decimal InitialRetailPrice { get; set; }
    public DateTime DateSet { get; set; }

    // Navigation property to BranchProduct (the parent)
    public BranchProduct? BranchProduct { get; set; }
}

}