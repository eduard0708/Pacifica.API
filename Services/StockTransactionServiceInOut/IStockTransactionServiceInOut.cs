// using Pacifica.API.Dtos.StockTransactionInOut;

// namespace Pacifica.API.Services.StockTransactionServiceInout
// {
//     public interface IStockTransactionServiceInOut
//     {
//         Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction);
//         Task<ApiResponse<List<GetStockTransactionInOutDto>>> GetAllTransactionsAsync();
//         Task<ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber);  // New method

//     }
// }
 
 using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Services.StockTransactionServiceInOut
{
    public interface IStockTransactionServiceInOut
    {
        Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction, int transactionType);
        Task<ApiResponse<List<GetStockTransactionInOutDto>>> GetAllTransactionsAsync();
        Task<ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber);
    }
}
