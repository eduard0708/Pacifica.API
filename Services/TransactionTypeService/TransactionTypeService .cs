using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Models;
using Pacifica.API.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pacifica.API.Services.TransactionReferenceService;

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
            var transactionTypes = await _context.TransactionTypes
                .Where(tt => tt.DeletedAt == null)
                .ToListAsync();

            if (transactionTypes == null || !transactionTypes.Any())
            {
                return new ApiResponse<IEnumerable<TransactionType>>
                {
                    Success = false,
                    Message = "No transaction types found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<TransactionType>>
            {
                Success = true,
                Message = "Transaction types retrieved successfully.",
                Data = transactionTypes
            };
        }

        public async Task<ApiResponse<TransactionType>> GetTransactionTypeByIdAsync(int id)
        {
            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.Id == id && tt.DeletedAt == null);

            if (transactionType == null)
            {
                return new ApiResponse<TransactionType>
                {
                    Success = false,
                    Message = "Transaction type not found.",
                    Data = null
                };
            }

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction type retrieved successfully.",
                Data = transactionType
            };
        }

        public async Task<ApiResponse<TransactionType>> CreateTransactionTypeAsync(TransactionType transactionType)
        {
            _context.TransactionTypes.Add(transactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction type created successfully.",
                Data = transactionType
            };
        }

        public async Task<ApiResponse<TransactionType>> UpdateTransactionTypeAsync(int id, TransactionType transactionType)
        {
            var existingTransactionType = await _context.TransactionTypes.FindAsync(id);
            if (existingTransactionType == null || existingTransactionType.DeletedAt != null)
            {
                return new ApiResponse<TransactionType>
                {
                    Success = false,
                    Message = "Transaction type not found or already deleted.",
                    Data = null
                };
            }

            existingTransactionType.TransactionTypeName = transactionType.TransactionTypeName;
            existingTransactionType.Description = transactionType.Description;
            existingTransactionType.UpdatedAt = DateTime.Now;
            existingTransactionType.UpdatedBy = transactionType.UpdatedBy;

            _context.TransactionTypes.Update(existingTransactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionType>
            {
                Success = true,
                Message = "Transaction type updated successfully.",
                Data = existingTransactionType
            };
        }

        public async Task<ApiResponse<bool>> DeleteTransactionTypeAsync(int id)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(id);
            if (transactionType == null || transactionType.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction type not found or already deleted.",
                    Data = false
                };
            }

            transactionType.DeletedAt = DateTime.Now;
            _context.TransactionTypes.Update(transactionType);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Transaction type deleted successfully.",
                Data = true
            };
        }
    }
}
