using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockOutReferenceService
{
    public class StockOutReferenceService : IStockOutReferenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockOutReferenceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<StockOutReference>>> GetAllReferencesStockOutAsync()
        {
            var stockOutReferences = await _context.StockOutReferences
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!stockOutReferences.Any())
            {
                return new ApiResponse<IEnumerable<StockOutReference>>
                {
                    Success = false,
                    Message = "No Stock-Out references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<StockOutReference>>
            {
                Success = true,
                Message = "Stock-Out references retrieved successfully.",
                Data = stockOutReferences
            };
        }

        public async Task<ApiResponse<StockOutReference>> GetReferencesStockOutByIdAsync(int id)
        {
            var stockOutReference = await _context.StockOutReferences
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (stockOutReference == null)
            {
                return new ApiResponse<StockOutReference>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<StockOutReference>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = stockOutReference
            };
        }

        public async Task<ApiResponse<StockOutReference>> CreateStockOutReferenceAsync(StockOutReference stockOutReference)
        {
            _context.StockOutReferences.Add(stockOutReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<StockOutReference>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = stockOutReference
            };
        }

        public async Task<ApiResponse<StockOutReference>> UpdateStockOutReferenceAsync(int id, StockOutReference stockOutReference)
        {
            var existingStockOutReference = await _context.StockOutReferences.FindAsync(id);
            if (existingStockOutReference == null || existingStockOutReference.DeletedAt != null)
            {
                return new ApiResponse<StockOutReference>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingStockOutReference.StockOutReferenceName = stockOutReference.StockOutReferenceName;
            existingStockOutReference.UpdatedAt = DateTime.Now;
            existingStockOutReference.UpdatedBy = stockOutReference.UpdatedBy;

            _context.StockOutReferences.Update(existingStockOutReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<StockOutReference>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingStockOutReference
            };
        }

        public async Task<ApiResponse<bool>> DeleteStockOutReferenceAsync(int id)
        {
            var stockOutReference = await _context.StockOutReferences.FindAsync(id);
            if (stockOutReference == null || stockOutReference.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            stockOutReference.DeletedAt = DateTime.Now;
            _context.StockOutReferences.Update(stockOutReference);
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
