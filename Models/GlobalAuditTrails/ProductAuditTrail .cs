namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class ProductAuditTrail : AuditTrail<Product>
    {
        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}