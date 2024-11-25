
namespace Pacifica.API.Models.Transaction
{
    public class StockInReference: AuditDetails
    {
        public int Id { get; set; }
        public string? StockInReferenceName { get; set; }
        public ICollection<StockIn>? StockIns { get; set; }  // One-to-many relationship with Products

    }
}