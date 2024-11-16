using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.StatusService
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StatusService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<Status>>> GetAllStatusesAsync()
        {
            var Status = await _context.Statuses
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!Status.Any())
            {
                return new ApiResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = "No transaction references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<Status>>
            {
                Success = true,
                Message = "Transaction references retrieved successfully.",
                Data = Status
            };
        }

        public async Task<ApiResponse<Status>> GetStatusByIdAsync(int id)
        {
            var Status = await _context.Statuses
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (Status == null)
            {
                return new ApiResponse<Status>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = Status
            };
        }

        public async Task<ApiResponse<Status>> CreateStatusAsync(Status Status)
        {
            _context.Statuses.Add(Status);
            await _context.SaveChangesAsync();

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = Status
            };
        }

        public async Task<ApiResponse<Status>> UpdateStatusAsync(int id, Status Status)
        {
            var existingStatus = await _context.Statuses.FindAsync(id);
            if (existingStatus == null || existingStatus.DeletedAt != null)
            {
                return new ApiResponse<Status>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingStatus.StatusName = Status.StatusName;
            existingStatus.Description = Status.Description;
            existingStatus.UpdatedAt = DateTime.Now;
            existingStatus.UpdatedBy = Status.UpdatedBy;

            _context.Statuses.Update(existingStatus);
            await _context.SaveChangesAsync();

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingStatus
            };
        }

        public async Task<ApiResponse<bool>> DeleteStatusAsync(int id)
        {
            var Status = await _context.TransactionTypes.FindAsync(id);
            if (Status == null || Status.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            Status.DeletedAt = DateTime.Now;
            _context.TransactionTypes.Update(Status);
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






