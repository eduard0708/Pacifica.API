using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Services.StockTransactionServiceInout
{
    public interface IStockTransactionServiceInOut
    {
        Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction);
    }
}
