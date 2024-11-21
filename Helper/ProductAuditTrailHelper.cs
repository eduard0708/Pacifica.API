using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Helper
{
    public class ProductAuditTrailHelper
    {
        private readonly ApplicationDbContext _context;

        public ProductAuditTrailHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generic method for creating an audit trail
        public async Task CreateAuditTrailAsync(int productId, string action, string newValue, string actionBy, string? remarks = null)
        {
            var auditTrail = new ProductAuditTrail
            {
                ProductId = productId,
                Action = action,
                NewValue = newValue,
                ActionBy = actionBy,
                ActionDate = DateTime.Now,
                Remarks = remarks
            };

            // Add the audit trail to the context
            await _context.ProductAuditTrails.AddAsync(auditTrail);
        }

        // Method for logging a "Created" action
        public async Task LogCreateAuditAsync(Product product, string createdBy)
        {
            var newValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}";
            await CreateAuditTrailAsync(product.Id, "Created", newValue, createdBy);
        }

        // Method for logging an "Updated" action
        public async Task LogUpdateAuditAsync(Product oldProduct, Product newProduct, string updatedBy)
        {
            var newValue = $"ProductName: {newProduct.ProductName}, SKU: {newProduct.SKU}";
            var oldValue = $"ProductName: {oldProduct.ProductName}, SKU: {oldProduct.SKU}"; // Save old values as well
            var remarks = $"Old Value: {oldValue}";
            await CreateAuditTrailAsync(newProduct.Id, "Updated", newValue, updatedBy, remarks);
        }

        // Method for logging a "Deleted" action
        public async Task LogDeleteAuditAsync(Product product, string deletedBy, string? remarks = null)
        {
            var newValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}";
            await CreateAuditTrailAsync(product.Id, "Deleted", newValue, deletedBy, remarks);
        }
    }

}