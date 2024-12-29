
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Helper

{
    public static class BranchProductAuditTrailHelper
    {
        // Method to create dictionaries for old and new values
        public static (Dictionary<string, object> newValues, Dictionary<string, object> oldValues) CreateAuditTrailValues(UpdateBranchProductDto newDto, BranchProduct oldEntity)
        {
            // Create dictionaries to hold the new and old values
            var newValues = new Dictionary<string, object>();
            var oldValues = new Dictionary<string, object>();

            // Manually adding values for the UpdateBranchProductDto (new values)
            newValues.Add("CostPrice", newDto.CostPrice);
            newValues.Add("RetailPrice", newDto.RetailPrice);
            newValues.Add("StockQuantity", newDto.StockQuantity);
            newValues.Add("ReorderLevel", newDto.ReorderLevel);
            newValues.Add("MinStockLevel", newDto.MinStockLevel);
            newValues.Add("Remarks", newDto.Remarks!);

            // Manually adding values for the BranchProduct (old values)
            oldValues.Add("CostPrice", oldEntity.CostPrice);
            oldValues.Add("RetailPrice", oldEntity.RetailPrice);
            oldValues.Add("StockQuantity", oldEntity.StockQuantity);
            oldValues.Add("ReorderLevel", oldEntity.ReorderLevel);
            oldValues.Add("MinStockLevel", oldEntity.MinStockLevel);
            oldValues.Add("Remarks", oldEntity.Remarks!);

            return (newValues, oldValues);
        }

    
        // Method to create an audit trail entry
        public static BranchProductAuditTrail CreateAuditTrailEntry(
            BranchProduct existingBranchProduct,
            string action,
            string updatedBy,
            string newValueJson,
            string oldValueJson,
            string remarks)
        {
            // Create the audit trail object
            var auditTrail = new BranchProductAuditTrail
            {
                BranchId = existingBranchProduct.BranchId,
                ProductId = existingBranchProduct.ProductId,
                Action = action,
                ActionDate = DateTime.UtcNow,
                ActionBy = updatedBy,
                NewValue = newValueJson,
                OldValue = oldValueJson,
                Remarks = remarks
            };

            return auditTrail;
        }
    }
}