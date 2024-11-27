using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockOutReferenceService
{
    public interface IStockOutReferenceService
    {
        // Retrieves all StockOutReference records
        Task<ApiResponse<IEnumerable<StockOutReference>>> GetAllReferencesStockOutAsync();

        // Retrieves a single StockOutReference record by its ID
        Task<ApiResponse<StockOutReference>> GetReferencesStockOutByIdAsync(int id);

        // Creates a new StockOutReference record
        Task<ApiResponse<StockOutReference>> CreateStockOutReferenceAsync(StockOutReference stockOutReference);

        // Updates an existing StockOutReference record by its ID
        Task<ApiResponse<StockOutReference>> UpdateStockOutReferenceAsync(int id, StockOutReference stockOutReference);

        // Deletes a StockOutReference record by its ID
        Task<ApiResponse<bool>> DeleteStockOutReferenceAsync(int id);
    }
}
