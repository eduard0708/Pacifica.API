using Pacifica.API.Dtos.F152Report;


namespace Pacifica.API.Services.F152ReportService
{
    public interface IF152ReportService
    {
        // Transaction CRUD operations
        Task<IEnumerable<F152ReportTransactionDto>> GetAllTransactionsAsync();
        Task<F152ReportTransactionDto> GetTransactionByIdAsync(int id);
        Task<F152ReportTransactionDto> CreateTransactionAsync(F152ReportTransactionDto transactionDto);
        Task<bool> UpdateTransactionAsync(int id, F152ReportTransactionDto transactionDto);
        Task<bool> DeleteTransactionAsync(int id);

        // Category CRUD operations
        Task<IEnumerable<F152ReportCategoryDto>> GetCategoriesByTransactionIdAsync(int transactionId);
        Task<F152ReportCategoryDto> CreateCategoryAsync(int transactionId, F152ReportCategoryDto categoryDto);
        Task<bool> UpdateCategoryAsync(int id, F152ReportCategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}