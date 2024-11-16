namespace Pacifica.API.Dtos.TransactionReference
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string? StatusName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}