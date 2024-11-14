using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.TransactionTypeService
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionTypeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<TransactionType>>> GetAllTransactionTypesAsync()
        {
            var transactionType = await _context.TransactionTypes
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!transactionType.Any())
            {
                return new ApiResponse<IEnumerable<TransactionType>>
                {
                    Success = false,
                    Message = "No transaction references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<TransactionType>>
            {
                Success = true,
                Message = "Transaction references retrieved successfully.",
                Data = transactionType
            };
        }

        public async Task<ApiResponse<TransactionType>> GetTransactionTypeByIdAsync(int id)
        {
            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (transactionType == null)
            {
                return new ApiResponse<TransactionType>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = transactionType
            };
        }

        public async Task<ApiResponse<TransactionType>> CreateTransactionTypeAsync(TransactionType TransactionType)
        {
            _context.TransactionTypes.Add(TransactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = TransactionType
            };
        }

        public async Task<ApiResponse<TransactionType>> UpdateTransactionTypeAsync(int id, TransactionType TransactionType)
        {
            var existingTransactionType = await _context.TransactionTypes.FindAsync(id);
            if (existingTransactionType == null || existingTransactionType.DeletedAt != null)
            {
                return new ApiResponse<TransactionType>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingTransactionType.TransactionTypeName = TransactionType.TransactionTypeName;
            existingTransactionType.Description = TransactionType.Description;
            existingTransactionType.UpdatedAt = DateTime.Now;
            existingTransactionType.UpdatedBy = TransactionType.UpdatedBy;

            _context.TransactionTypes.Update(existingTransactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingTransactionType
            };
        }

        public async Task<ApiResponse<bool>> DeleteTransactionTypeAsync(int id)
        {
            var TransactionType = await _context.TransactionTypes.FindAsync(id);
            if (TransactionType == null || TransactionType.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            TransactionType.DeletedAt = DateTime.Now;
            _context.TransactionTypes.Update(TransactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Transaction reference deleted successfully.",
                Data = true
            };
        }
    }
}






