using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.TransactionTypeService
{
    public interface ITransactionTypeService
    {
        Task<ApiResponse<IEnumerable<TransactionType>>> GetAllTransactionTypesAsync();
        Task<ApiResponse<TransactionType>> GetTransactionTypeByIdAsync(int id);
        Task<ApiResponse<TransactionType>> CreateTransactionTypeAsync(TransactionType transactionType);
        Task<ApiResponse<TransactionType>> UpdateTransactionTypeAsync(int id, TransactionType transactionType);
        Task<ApiResponse<bool>> DeleteTransactionTypeAsync(int id);
    }
}
