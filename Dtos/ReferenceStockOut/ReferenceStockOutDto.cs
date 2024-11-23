namespace Pacifica.API.Dtos.ReferenceStockOut
{
    public class ReferenceStockOutDto
    {
        public int Id { get; set; }
        public string? ReferenceStockOutName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}