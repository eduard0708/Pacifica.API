using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Inventory
{
    public class BranchProductInventoryAuditTrailDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int PreviousStockQuantity { get; set; }
        public int AdjustedQuantity { get; set; }
        public string? AdjustedBy { get; set; }
        public DateTime DateAdjusted { get; set; }
        public string? AdjustmentReason { get; set; }
    }
}