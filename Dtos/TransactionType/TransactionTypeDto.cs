namespace Pacifica.API.Dtos.TransactionType
{
     public class TransactionTypeDto
    {
        public int Id { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}