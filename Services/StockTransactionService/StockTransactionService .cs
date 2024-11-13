using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Services.StockTransactionService
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly ApplicationDbContext _context;
        public IMapper _mapper { get; }

        public StockTransactionService(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction)
        {
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == transaction.BranchId && bp.ProductId == transaction.ProductId);

            if (branchProduct == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Branch product not found.",
                    Data = null
                };
            }

            // Adjust stock quantity based on transaction type
            if (transaction.TransactionType == StockTransactionType.StockIn)
            {
                branchProduct.StockQuantity += transaction.StockQuantity;
            }
            else if (transaction.TransactionType == StockTransactionType.StockOut)
            {
                if (branchProduct.StockQuantity < transaction.StockQuantity)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Insufficient stock quantity.",
                        Data = null
                    };
                }
                branchProduct.StockQuantity -= transaction.StockQuantity;
            }
            else
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid transaction type.",
                    Data = null
                };
            }

            try
            {

                var createStock = _mapper.Map<StockTransactionInOut>(transaction);
                // Save transaction and update stock quantity
                _context.StockTransactionInOuts.Add(createStock);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Transaction processed successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error processing transaction: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
