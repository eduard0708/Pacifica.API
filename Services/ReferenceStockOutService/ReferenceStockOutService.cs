using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.ReferenceStockOutService
{
    public class ReferenceStockOutService : IReferenceStockOutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReferenceStockOutService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ReferenceStockOut>>> GetAllReferencesStockOutAsync()
        {
            var ReferenceStockOuts = await _context.ReferenceStockOuts
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!ReferenceStockOuts.Any())
            {
                return new ApiResponse<IEnumerable<ReferenceStockOut>>
                {
                    Success = false,
                    Message = "No Stock-In references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<ReferenceStockOut>>
            {
                Success = true,
                Message = "Stock-In references retrieved successfully.",
                Data = ReferenceStockOuts
            };
        }

        public async Task<ApiResponse<ReferenceStockOut>> GetReferenceStockOutByIdAsync(int id)
        {
            var ReferenceStockOut = await _context.ReferenceStockOuts
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (ReferenceStockOut == null)
            {
                return new ApiResponse<ReferenceStockOut>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<ReferenceStockOut>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = ReferenceStockOut
            };
        }

        public async Task<ApiResponse<ReferenceStockOut>> CreateReferenceStockOutAsync(ReferenceStockOut referenceStockOut)
        {
            _context.ReferenceStockOuts.Add(referenceStockOut);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReferenceStockOut>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = referenceStockOut
            };
        }

        public async Task<ApiResponse<ReferenceStockOut>> UpdateReferenceStockOutAsync(int id, ReferenceStockOut referenceStockOut)
        {
            var existingReferenceStockOut = await _context.ReferenceStockOuts.FindAsync(id);
            if (existingReferenceStockOut == null || existingReferenceStockOut.DeletedAt != null)
            {
                return new ApiResponse<ReferenceStockOut>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingReferenceStockOut.ReferenceStockOutName = referenceStockOut.ReferenceStockOutName;
            existingReferenceStockOut.Description = referenceStockOut.Description;
            existingReferenceStockOut.UpdatedAt = DateTime.Now;
            existingReferenceStockOut.UpdatedBy = referenceStockOut.UpdatedBy;

            _context.ReferenceStockOuts.Update(existingReferenceStockOut);
            await _context.SaveChangesAsync();

            return new ApiResponse<ReferenceStockOut>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingReferenceStockOut
            };
        }

        public async Task<ApiResponse<bool>> DeleteReferenceStockOutAsync(int id)
        {
            var referenceStockOut = await _context.ReferenceStockOuts.FindAsync(id);
            if (referenceStockOut == null || referenceStockOut.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            referenceStockOut.DeletedAt = DateTime.Now;
            _context.ReferenceStockOuts.Update(referenceStockOut);
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






