using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockInReference;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.StockInReferenceService
{
    public class StockInReferenceService : IStockInReferenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockInReferenceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<StockInReference>>> GetAllReferencesStockInAsync()
        {
            var StockInReferences = await _context.StockInReferences
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!StockInReferences.Any())
            {
                return new ApiResponse<IEnumerable<StockInReference>>
                {
                    Success = false,
                    Message = "No Stock-In references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<StockInReference>>
            {
                Success = true,
                Message = "Stock-In references retrieved successfully.",
                Data = StockInReferences
            };
        }

        public async Task<ApiResponse<IEnumerable<SelectReferenceStockInDTO>>> GetSelectStockInsAsync()
        {
            var StockInReferences = await _context.StockInReferences
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();


            if (!StockInReferences.Any())
            {
                return new ApiResponse<IEnumerable<SelectReferenceStockInDTO>>
                {
                    Success = false,
                    Message = "No Stock-In references found.",
                    Data = null
                };
            }
            var selectReferenceStockIns = _mapper.Map<SelectReferenceStockInDTO[]>(StockInReferences);

            return new ApiResponse<IEnumerable<SelectReferenceStockInDTO>>
            {
                Success = true,
                Message = "Stock-In references retrieved successfully.",
                Data = selectReferenceStockIns
            };
        }

        public async Task<ApiResponse<StockInReference>> GetReferencesStockInByIdAsync(int id)
        {
            var StockInReference = await _context.StockInReferences
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (StockInReference == null)
            {
                return new ApiResponse<StockInReference>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<StockInReference>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = StockInReference
            };
        }

        public async Task<ApiResponse<StockInReference>> CreateStockInReferenceAsync(StockInReference StockInReference)
        {
            _context.StockInReferences.Add(StockInReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<StockInReference>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = StockInReference
            };
        }

        public async Task<ApiResponse<StockInReference>> UpdateStockInReferenceAsync(int id, StockInReference StockInReference)
        {
            var existingStockInReference = await _context.StockInReferences.FindAsync(id);
            if (existingStockInReference == null || existingStockInReference.DeletedAt != null)
            {
                return new ApiResponse<StockInReference>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingStockInReference.StockInReferenceName = StockInReference.StockInReferenceName;
            existingStockInReference.UpdatedAt = DateTime.Now;
            existingStockInReference.UpdatedBy = StockInReference.UpdatedBy;

            _context.StockInReferences.Update(existingStockInReference);
            await _context.SaveChangesAsync();

            return new ApiResponse<StockInReference>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingStockInReference
            };
        }

        public async Task<ApiResponse<bool>> DeleteStockInReferenceAsync(int id)
        {
            var StockInReference = await _context.StockInReferences.FindAsync(id);
            if (StockInReference == null || StockInReference.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            StockInReference.DeletedAt = DateTime.Now;
            _context.StockInReferences.Update(StockInReference);
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






