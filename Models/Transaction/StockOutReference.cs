namespace Pacifica.API.Models.Transaction
{
    public class StockOutReference : AuditDetails
    {
        public int Id { get; set; }
        public string? StockOutReferenceName { get; set; }
        public ICollection<StockOut>? StockOuts { get; set; }  // One-to-many relationship with Products

    }
}