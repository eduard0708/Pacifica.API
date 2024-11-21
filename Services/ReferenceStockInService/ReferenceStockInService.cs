using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.ReferenceStockInService
{
    public class ReferenceStockInService : IReferenceStockInService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReferenceStockInService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ReferenceStockIn>>> GetAllReferencesStockInAsync()
        {
            var ReferenceStockIns = await _context.ReferenceStockIns
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!ReferenceStockIns.Any())
            {
                return new ApiResponse<IEnumerable<ReferenceStockIn>>
                {
                    Success = false,
                    Message = "No Stock-In references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<ReferenceStockIn>>
            {
                Success = true,
                Message = "Stock-In references retrieved successfully.",
                Data = ReferenceStockIns
            };
        }

        public async Task<ApiResponse<ReferenceStockIn>> GetReferencesStockInByIdAsync(int id)
        {
            var ReferenceStockIn = await _context.ReferenceStockIns
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (ReferenceStockIn == null)
            {
                return new ApiResponse<ReferenceStockIn>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<ReferenceStockIn>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = ReferenceStockIn
            };
        }

        public async Task<ApiResponse<ReferenceStockIn>> CreateReferenceStockInAsync(ReferenceStockIn ReferenceStockIn)
        {
            _context.ReferenceStockIns.Add(ReferenceStockIn);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReferenceStockIn>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = ReferenceStockIn
            };
        }

        public async Task<ApiResponse<ReferenceStockIn>> UpdateReferenceStockInAsync(int id, ReferenceStockIn ReferenceStockIn)
        {
            var existingReferenceStockIn = await _context.ReferenceStockIns.FindAsync(id);
            if (existingReferenceStockIn == null || existingReferenceStockIn.DeletedAt != null)
            {
                return new ApiResponse<ReferenceStockIn>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingReferenceStockIn.ReferenceStockInName = ReferenceStockIn.ReferenceStockInName;
            existingReferenceStockIn.Description = ReferenceStockIn.Description;
            existingReferenceStockIn.UpdatedAt = DateTime.Now;
            existingReferenceStockIn.UpdatedBy = ReferenceStockIn.UpdatedBy;

            _context.ReferenceStockIns.Update(existingReferenceStockIn);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReferenceStockIn>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingReferenceStockIn
            };
        }

        public async Task<ApiResponse<bool>> DeleteReferenceStockInAsync(int id)
        {
            var ReferenceStockIn = await _context.ReferenceStockIns.FindAsync(id);
            if (ReferenceStockIn == null || ReferenceStockIn.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            ReferenceStockIn.DeletedAt = DateTime.Now;
            _context.ReferenceStockIns.Update(ReferenceStockIn);
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






