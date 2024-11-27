namespace Pacifica.API.Dtos.StockInReference
{
    public class StockInReferenceDto :AuditDetails
    {
        public int Id { get; set; }
        public string? StockInReferenceName { get; set; }
    }
}