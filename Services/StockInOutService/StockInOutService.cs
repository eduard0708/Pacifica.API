using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockInOut;
namespace Pacifica.API.Services.StockInOutService
{
    public class StockInOutService : IStockInOutService
    {
        private readonly ApplicationDbContext _context;
        public IMapper _mapper { get; }

        public StockInOutService(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // public async Task<ApiResponse<string>> StockInOutAsync(CreateStockInOutDto transaction)
        // {
        //     var branchProduct = await _context.BranchProducts
        //         .FirstOrDefaultAsync(bp => bp.BranchId == transaction.BranchId && bp.ProductId == transaction.ProductId);

        //     if (branchProduct == null)
        //     {
        //         return new ApiResponse<string>
        //         {
        //             Success = false,
        //             Message = "Branch product not found.",
        //             Data = null
        //         };
        //     }

        //     // Adjust stock quantity based on transactionType parameter
        //     if (transaction.TransactionTypeId == 1) //Value 1 is the ID of Transaction In Add Stock
        //     {
        //         branchProduct.StockQuantity += transaction.StockQuantity;
        //     }
        //     else if (transaction.TransactionTypeId == 2) //Value 2 is the ID of Transaction In Deduct Stock
        //     {
        //         if (branchProduct.StockQuantity < transaction.StockQuantity)
        //         {
        //             return new ApiResponse<string>
        //             {
        //                 Success = false,
        //                 Message = "Insufficient stock quantity.",
        //                 Data = null
        //             };
        //         }

        //         branchProduct.StockQuantity -= transaction.StockQuantity; //Deduct to the branch Stock
        //     }
        //     else
        //     {
        //         return new ApiResponse<string>
        //         {
        //             Success = false,
        //             Message = "Invalid transaction type.",
        //             Data = null
        //         };
        //     }

        //     try
        //     {

        //         var createStock = _mapper.Map<StockInOut>(transaction);

        //         // Save transaction and update stock quantity
        //         _context.StockInOuts.Add(createStock);
        //         await _context.SaveChangesAsync();

        //         return new ApiResponse<string>
        //         {
        //             Success = true,
        //             Message = "Transaction processed successfully.",
        //             Data = null
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ApiResponse<string>
        //         {
        //             Success = false,
        //             Message = $"Error processing transaction: {ex.InnerException?.Message}",
        //             Data = null
        //         };
        //     }
        // }

        public async Task<ApiResponse<string>> CreateStockInOutAsync(CreateStockInOutDto transaction)
        {
            // Check if a transaction with the same branchId, transactionTypeId, productId, and transactionReferenceId exists
            var existingTransaction = await _context.StockInOuts
                .FirstOrDefaultAsync(t => t.BranchId == transaction.BranchId &&
                                          t.TransactionTypeId == transaction.TransactionTypeId &&
                                          t.ProductId == transaction.ProductId &&
                                          t.ReferenceStockIn!.Id == transaction.ReferenceStockInId && 
                                          t.ReferenceStockOut!.Id == transaction.ReferenceStockOutId);

            if (existingTransaction != null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Transaction with the same reference already exists.",
                    Data = null
                };
            }

            // Retrieve the branch product to check stock quantity
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

            // Adjust stock quantity based on transactionTypeId
            if (transaction.TransactionTypeId == 1) // Transaction In (Add Stock)
            {
                branchProduct.StockQuantity += transaction.StockQuantity;
            }
            else if (transaction.TransactionTypeId == 2) // Transaction Out (Deduct Stock)
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

                branchProduct.StockQuantity -= transaction.StockQuantity; // Deduct stock from the branch
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
                // Map DTO to entity for the transaction
                var stockTransaction = _mapper.Map<StockInOut>(transaction);

                // Save transaction to the database
                _context.StockInOuts.Add(stockTransaction);

                // Save changes to update the stock quantity and create the transaction
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


        public async Task<ApiResponse<List<GetStockInOutDto>>> GetAllTransactionsAsync()
        {
            var transactions = await _context.StockInOuts
                .Select(t => _mapper.Map<GetStockInOutDto>(t))
                .ToListAsync();

            return new ApiResponse<List<GetStockInOutDto>>
            {
                Success = true,
                Message = "Transactions retrieved successfully.",
                Data = transactions
            };
        }

        public async Task<ApiResponse<List<GetByReferenceNumberStockInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber)
        {
            var transactions = await _context.StockInOuts
                .Where(t => t.TransactionNumber == referenceNumber)
                .Join(_context.Branches, t => t.BranchId, b => b.Id, (t, b) => new { t, b })
                .Join(_context.Products, tb => tb.t.ProductId, p => p.Id, (tb, p) => new { tb.t, tb.b, p })
                .Join(_context.ReferenceStockIns, tbi => tbi.t.ReferenceStockInId, tr => tr.Id, (tbi, tr) => new { tbi.t, tbi.b, tbi.p, tr })
                .Join(_context.ReferenceStockOuts, tbo => tbo.t.ReferenceStockOutId, tr => tr.Id, (tbo, tr) => new { tbo.t, tbo.b, tbo.p, tr })
                .Select(x => new GetByReferenceNumberStockInOutDto
                {
                    Id = x.t.Id,
                    BranchId = x.t.BranchId,
                    BranchName = x.b.BranchName,
                    ProductId = x.t.ProductId,
                    ProductName = x.p.ProductName,
                    ProductCategoryId = x.p.Category!.Id,
                    ProductCategory = x.p.Category!.CategoryName,
                    ReferenceStockInId = x.t.ReferenceStockInId,
                    ReferenceStockInName = x.t.ReferenceStockIn!.ReferenceStockInName ?? "unknown",
                    ReferenceStockOutId = x.t.ReferenceStockOutId,
                    ReferenceStockOutName = x.t.ReferenceStockOut!.ReferenceStockOutName ?? "unknown",
                    TransactionDate = x.t.TransactionDate,
                    TransactionTypeId = x.t.TransactionTypeId,
                    TransactionTypeName = x.t.TransactionType!.TransactionTypeName ?? "unknown",
                    TransactionNumber = x.t.TransactionNumber,
                    StockQuantity = x.t.StockQuantity,
                    DateReported = x.t.DateReported,
                    Remarks = x.t.Remarks,
                })
                .ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return new ApiResponse<List<GetByReferenceNumberStockInOutDto>>
                {
                    Success = false,
                    Message = "Transaction not found.",
                    Data = null
                };
            }

            return new ApiResponse<List<GetByReferenceNumberStockInOutDto>>
            {
                Success = true,
                Message = "Transactions retrieved successfully.",
                Data = transactions
            };
        }

    }
}
