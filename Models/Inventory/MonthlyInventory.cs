using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Inventory
{
    public class MonthlyInventory
    {
        [Key]
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int ProductId { get; set; }

    public int StockQuantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public DateTime Date { get; set; }

    // Navigation property to BranchProduct
    public BranchProduct? BranchProduct { get; set; }
    }
}