namespace Pacifica.API.Models.Transaction
{
    public class PaymentMethod : AuditDetails
    {
        public int Id { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? Description { get; set; }
        public ICollection<StockOut>? StockOuts { get; set; }  // One-to-many relationship with Products
    }
}