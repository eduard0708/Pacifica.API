namespace Pacifica.API.Dtos.TransactionType
{
     public class UpdateTransactionTypeDto
    {
        public string? TransactionTypeName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}