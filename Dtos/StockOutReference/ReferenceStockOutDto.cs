namespace Pacifica.API.Dtos.StockOutReference
{
    public class StockOutReferenceDto :AuditDetails
    {
        public int Id { get; set; }
        public string? StockOutReferenceName { get; set; }
    }
}