
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Dtos.StockOut;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockOutService
{
    public interface IStockOutService
    {
        Task<ApiResponse<IEnumerable<StockOutDTO>>> GetAllStockOutsAsync();
        Task<ApiResponse<StockOutDTO>> GetStockOutByIdAsync(int id);
        Task<ApiResponse<IEnumerable<StockOutDTO>>> GetStockOutByReferenceNumberAsync(string referenceNumber);
        Task<ApiResponse<StockOutDTO>> CreateStockOutAsync(CreateStockOutDTO stockOutDto);
        Task<ApiResponse<IEnumerable<StockOutDTO>>> CreateMultipleStockOutAsync(IEnumerable<CreateStockOutDTO> stockOutDtos);
        Task<ApiResponse<StockOutDTO>> UpdateStockOutAsync(int id, StockOutUpdateDTO stockOutDto);
        Task<ApiResponse<bool>> DeleteStockOutAsync(StockOutDeleteParams deleteParams);
        Task<ApiResponse<bool>> RestoreStockOutAsync(StockOutRestoreParams restoreParams);
        Task<ApiResponse<List<StockOut>>> GetAllDeletedStockOutAsync();
        Task<ApiResponse<IEnumerable<ViewStockOutDTO>>> GetByDateRangeOrRefenceAsync(
          string referenceNumber,
          DateTime? dateCreatedStart = null,
          DateTime? dateCreatedEnd = null,
          DateTime? dateSoldStart = null,
          DateTime? dateSoldEnd = null);
    }
}
