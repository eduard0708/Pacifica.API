using Pacifica.API.Dtos.StockOutReference;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockOutReferenceService
{
    public interface IStockOutReferenceService
    {
        Task<ApiResponse<IEnumerable<StockOutReference>>> GetAllReferencesStockOutAsync();
        Task<ApiResponse<IEnumerable<SelectReferenceStockOutDTO>>> GetSelectStockOutsAsync();
        Task<ApiResponse<StockOutReference>> GetReferencesStockOutByIdAsync(int id);
        Task<ApiResponse<StockOutReference>> CreateStockOutReferenceAsync(StockOutReference stockOutReference);
        Task<ApiResponse<StockOutReference>> UpdateStockOutReferenceAsync(int id, StockOutReference stockOutReference);
        Task<ApiResponse<bool>> DeleteStockOutReferenceAsync(int id);
    }
}
