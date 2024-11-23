namespace Pacifica.API.Dtos.ReferenceStockIn
{
    public class ReferenceStockInDto
    {
        public int Id { get; set; }
        public string? ReferenceStockInName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}