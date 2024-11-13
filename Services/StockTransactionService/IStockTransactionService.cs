using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Services.StockTransactionService
{
    public interface IStockTransactionService
    {
        Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction);
    }
}
