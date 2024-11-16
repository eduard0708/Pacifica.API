using Pacifica.API.Dtos.StockInOut;

namespace Pacifica.API.Services.StockInOutService
{
    public interface IStockInOutService
    {
        Task<ApiResponse<string>> CreateStockInOutAsync(CreateStockInOutDto transaction);
        Task<ApiResponse<List<GetStockInOutDto>>> GetAllTransactionsAsync();
        Task<ApiResponse<List<GetByReferenceNumberStockInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber);
    }
}
