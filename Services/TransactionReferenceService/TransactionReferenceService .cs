using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Models;
using PacificaAPI.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.TransactionReferenceService
{
    public class TransactionReferenceService : ITransactionReferenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionReferenceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<TransactionReference>>> GetAllTransactionReferencesAsync()
        {
            var transactionReferences = await _context.TransactionReferences
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!transactionReferences.Any())
            {
                return new ApiResponse<IEnumerable<TransactionReference>>
                {
                    Success = false,
                    Message = "No transaction references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<TransactionReference>>
            {
                Success = true,
                Message = "Transaction references retrieved successfully.",
                Data = transactionReferences
            };
        }

        public async Task<ApiResponse<TransactionReference>> GetTransactionReferenceByIdAsync(int id)
        {
            var transactionReference = await _context.TransactionReferences
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (transactionReference == null)
            {
                return new ApiResponse<TransactionReference>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<TransactionReference>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = transactionReference
            };
        }

        public async Task<ApiResponse<TransactionReference>> CreateTransactionReferenceAsync(TransactionReference transactionReference)
        {
            _context.TransactionReferences.Add(transactionReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionReference>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = transactionReference
            };
        }

        public async Task<ApiResponse<TransactionReference>> UpdateTransactionReferenceAsync(int id, TransactionReference transactionReference)
        {
            var existingTransactionReference = await _context.TransactionReferences.FindAsync(id);
            if (existingTransactionReference == null || existingTransactionReference.DeletedAt != null)
            {
                return new ApiResponse<TransactionReference>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingTransactionReference.TransactionReferenceName = transactionReference.TransactionReferenceName;
            existingTransactionReference.Description = transactionReference.Description;
            existingTransactionReference.UpdatedAt = DateTime.Now;
            existingTransactionReference.UpdatedBy = transactionReference.UpdatedBy;

            _context.TransactionReferences.Update(existingTransactionReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<TransactionReference>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingTransactionReference
            };
        }

        public async Task<ApiResponse<bool>> DeleteTransactionReferenceAsync(int id)
        {
            var transactionReference = await _context.TransactionReferences.FindAsync(id);
            if (transactionReference == null || transactionReference.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            transactionReference.DeletedAt = DateTime.Now;
            _context.TransactionReferences.Update(transactionReference);
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






