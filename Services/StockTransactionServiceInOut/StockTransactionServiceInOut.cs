using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Services.StockTransactionServiceInOut
{
    public class StockTransactionServiceInOut : IStockTransactionServiceInOut
    {
        private readonly ApplicationDbContext _context;
        public IMapper _mapper { get; }

        public StockTransactionServiceInOut(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction, int transactionType)
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

            // Adjust stock quantity based on transactionType parameter
            if (transactionType == 1) //Value 1 is the ID of Transaction In Add Stock
            {
                branchProduct.StockQuantity += transaction.StockQuantity;
            }
            else if (transactionType == 2) //Value 2 is the ID of Transaction In Deduct Stock
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

                branchProduct.StockQuantity -= transaction.StockQuantity; //Deduct to the branch Stock
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
                transaction.TransactionTypeId = transactionType;
                var createStock = _mapper.Map<StockTransactionInOut>(transaction);
                createStock.StockTransactionType = transactionType;

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
                    Message = $"Error processing transaction: {ex.InnerException?.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<GetStockTransactionInOutDto>>> GetAllTransactionsAsync()
        {
            var transactions = await _context.StockTransactionInOuts
                .Select(t => _mapper.Map<GetStockTransactionInOutDto>(t))
                .ToListAsync();

            return new ApiResponse<List<GetStockTransactionInOutDto>>
            {
                Success = true,
                Message = "Transactions retrieved successfully.",
                Data = transactions
            };
        }

        public async Task<ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber)
        {
            var transactions = await _context.StockTransactionInOuts
                .Where(t => t.TransactionNumber == referenceNumber)
                .Join(_context.Branches, t => t.BranchId, b => b.Id, (t, b) => new { t, b })
                .Join(_context.Products, tb => tb.t.ProductId, p => p.Id, (tb, p) => new { tb.t, tb.b, p })
                .Join(_context.TransactionReferences, tbp => tbp.t.TransactionReferenceId, tr => tr.Id, (tbp, tr) => new { tbp.t, tbp.b, tbp.p, tr })
                .Select(x => new GetByReferenceNumberStockTransactionInOutDto
                {
                    Id = x.t.Id,
                    BranchId = x.t.BranchId,
                    ProductId = x.t.ProductId,
                    ReferenceNumber = x.t.TransactionNumber,
                    TransactionNumber = x.t.TransactionNumber,
                    TransactionTypeId = x.t.StockTransactionType,
                    StockQuantity = x.t.StockQuantity,
                    TransactionDate = x.t.TransactionDate,
                    BranchName = x.b.BranchName,
                    ProductName = x.p.ProductName,
                    ProductCategory = x.p.Category!.CategoryName,
                    TransactionReferenceName = x.tr.TransactionReferenceName
                })
                .ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
                {
                    Success = false,
                    Message = "Transaction not found.",
                    Data = null
                };
            }

            return new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
            {
                Success = true,
                Message = "Transactions retrieved successfully.",
                Data = transactions
            };
        }
    }
}


// using Microsoft.EntityFrameworkCore;
// using Pacifica.API.Dtos.StockTransactionInOut;

// namespace Pacifica.API.Services.StockTransactionServiceInout
// {
//     public class StockTransactionServiceInOut : IStockTransactionServiceInOut
//     {
//         private readonly ApplicationDbContext _context;
//         public IMapper _mapper { get; }

//         public StockTransactionServiceInOut(ApplicationDbContext context, IMapper mapper)
//         {
//             _mapper = mapper;
//             _context = context;
//         }

//         public async Task<ApiResponse<string>> ProcessTransactionAsync(CreateStockTransactionInOutDto transaction)
//         {
//             var branchProduct = await _context.BranchProducts
//                 .FirstOrDefaultAsync(bp => bp.BranchId == transaction.BranchId && bp.ProductId == transaction.ProductId);

//             if (branchProduct == null)
//             {
//                 return new ApiResponse<string>
//                 {
//                     Success = false,
//                     Message = "Branch product not found.",
//                     Data = null
//                 };
//             }

//             // Adjust stock quantity based on transaction type
//             if (transaction.TransactionType == StockTransactionType.StockIn)
//             {
//                 branchProduct.StockQuantity += transaction.StockQuantity;
//             }
//             else if (transaction.TransactionType == StockTransactionType.StockOut)
//             {
//                 if (branchProduct.StockQuantity < transaction.StockQuantity)
//                 {
//                     return new ApiResponse<string>
//                     {
//                         Success = false,
//                         Message = "Insufficient stock quantity.",
//                         Data = null
//                     };
//                 }
//                 branchProduct.StockQuantity -= transaction.StockQuantity;
//             }
//             else
//             {
//                 return new ApiResponse<string>
//                 {
//                     Success = false,
//                     Message = "Invalid transaction type.",
//                     Data = null
//                 };
//             }

//             try
//             {

//                 var createStock = _mapper.Map<StockTransactionInOut>(transaction);
//                 createStock.StockTransactionType = transaction.TransactionType;

//                 // Save transaction and update stock quantity
//                 _context.StockTransactionInOuts.Add(createStock);
//                 await _context.SaveChangesAsync();

//                 return new ApiResponse<string>
//                 {
//                     Success = true,
//                     Message = "Transaction processed successfully.",
//                     Data = null
//                 };
//             }
//             catch (Exception ex)
//             {
//                 return new ApiResponse<string>
//                 {
//                     Success = false,
//                     Message = $"Error processing transaction: {ex.InnerException?.Message}",
//                     Data = null
//                 };
//             }
//         }

//         public async Task<ApiResponse<List<GetStockTransactionInOutDto>>> GetAllTransactionsAsync()
//         {
//             var transactions = await _context.StockTransactionInOuts
//                 .Select(t => _mapper.Map<GetStockTransactionInOutDto>(t))
//                 .ToListAsync();

//             return new ApiResponse<List<GetStockTransactionInOutDto>>
//             {
//                 Success = true,
//                 Message = "Transactions retrieved successfully.",
//                 Data = transactions
//             };
//         }

//         public async Task<ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>> GetTransactionByReferenceNumberAsync(int referenceNumber)
//         {
//             var transactions = await _context.StockTransactionInOuts
//                 .Where(t => t.TransactionNumber == referenceNumber)
//                 .Join(_context.Branches, t => t.BranchId, b => b.Id, (t, b) => new { t, b })
//                 .Join(_context.Products, tb => tb.t.ProductId, p => p.Id, (tb, p) => new { tb.t, tb.b, p })
//                 .Join(_context.TransactionReferences, tbp => tbp.t.TransactionReferenceId, tr => tr.Id, (tbp, tr) => new { tbp.t, tbp.b, tbp.p, tr })
//                 .Select(x => new GetByReferenceNumberStockTransactionInOutDto
//                 {
//                     Id = x.t.Id,
//                     BranchId = x.t.BranchId,
//                     ProductId = x.t.ProductId,
//                     ReferenceNumber = x.t.TransactionNumber,
//                     TransactionNumber = x.t.TransactionNumber,
//                     TransactionTypeId = x.t.StockTransactionType,
//                     // TransactionTypeName = x.
//                     StockQuantity = x.t.StockQuantity,
//                     TransactionDate = x.t.TransactionDate,
//                     BranchName = x.b.BranchName,  // Assuming branch name is stored in the Branch entity
//                     ProductName = x.p.ProductName,  // Assuming product name is stored in the Product entity
//                     ProductCategory = x.p.Category!.CategoryName,  // Assuming category is stored in the Product entity
//                     TransactionReferenceName = x.tr.TransactionReferenceName  // Assuming reference name is stored in the TransactionReference entity
//                 })
//                 .ToListAsync();

//             if (transactions == null || transactions.Count == 0)
//             {
//                 return new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
//                 {
//                     Success = false,
//                     Message = "Transaction not found.",
//                     Data = null
//                 };
//             }

//             return new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
//             {
//                 Success = true,
//                 Message = "Transactions retrieved successfully.",
//                 Data = transactions
//             };
//         }

//     }

// }
