using Pacifica.API.Dtos.StockInReference;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockInReferenceService
{
    public interface IStockInReferenceService
    {
        Task<ApiResponse<IEnumerable<StockInReference>>> GetAllReferencesStockInAsync();
        Task<ApiResponse<IEnumerable<SelectReferenceStockInDTO>>> GetSelectStockInsAsync();
        Task<ApiResponse<StockInReference>> GetReferencesStockInByIdAsync(int id);
        Task<ApiResponse<StockInReference>> CreateStockInReferenceAsync(StockInReference StockInReference);
        Task<ApiResponse<StockInReference>> UpdateStockInReferenceAsync(int id, StockInReference StockInReference);
        Task<ApiResponse<bool>> DeleteStockInReferenceAsync(int id);
    }
}