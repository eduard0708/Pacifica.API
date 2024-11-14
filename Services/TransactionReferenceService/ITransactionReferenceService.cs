namespace Pacifica.API.Services.TransactionReferenceService
{
    public interface ITransactionReferenceService
    {
        Task<ApiResponse<IEnumerable<TransactionReference>>> GetAllTransactionReferencesAsync();
        Task<ApiResponse<TransactionReference>> GetTransactionReferenceByIdAsync(int id);
        Task<ApiResponse<TransactionReference>> CreateTransactionReferenceAsync(TransactionReference transactionReference);
        Task<ApiResponse<TransactionReference>> UpdateTransactionReferenceAsync(int id, TransactionReference transactionReference);
        Task<ApiResponse<bool>> DeleteTransactionReferenceAsync(int id);
    }
}