using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Dtos.Status;
using System.Linq.Expressions;

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
            var statuses = await _context.Statuses
                .Where(s => s.DeletedAt == null) // Ensure soft delete is respected
                .ToListAsync();

            if (!statuses.Any())
            {
                return new ApiResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = "No statuses found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<Status>>
            {
                Success = true,
                Message = "Statuses retrieved successfully.",
                Data = statuses
            };
        }

        public async Task<ApiResponse<IEnumerable<SelectStatusDto>>> GetSelectStatusesAsync()
        {
            var statuses = await _context.Statuses
                .Where(s => s.DeletedAt == null)
                .Select(s => new SelectStatusDto
                {
                    Id = s.Id,
                    StatusName = s.StatusName
                })
                .ToListAsync();

            if (!statuses.Any())
            {
                return new ApiResponse<IEnumerable<SelectStatusDto>>
                {
                    Success = false,
                    Message = "No statuses found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<SelectStatusDto>>
            {
                Success = true,
                Message = "Statuses retrieved successfully.",
                Data = statuses
            };
        }

        public async Task<ApiResponse<IEnumerable<Status>>> GetStatusesByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            var totalCount = await _context.Statuses
                .IgnoreQueryFilters() // Ignore soft delete filter
                .CountAsync();

            IQueryable<Status> query = _context.Statuses
                .IgnoreQueryFilters(); // Ignore soft delete filter

            // Apply sorting dynamically
            query = sortOrder == 1 ? query.OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

            // Apply pagination
            var statuses = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<Status>>
            {
                Success = true,
                Message = "Statuses retrieved successfully.",
                Data = statuses,
                TotalCount = totalCount
            };
        }

        private Expression<Func<Status, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "statusName":
                    return x => x.StatusName!;
                case "createdAt":
                    return x => x.CreatedAt;
                case "isDeleted":
                    return x => x.IsDeleted!;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<Status>> GetStatusByIdAsync(int id)
        {
            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

            if (status == null)
            {
                return new ApiResponse<Status>
                {
                    Success = false,
                    Message = "Status not found.",
                    Data = null
                };
            }

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Status retrieved successfully.",
                Data = status
            };
        }

        public async Task<ApiResponse<Status>> CreateStatusAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Status created successfully.",
                Data = status
            };
        }

        public async Task<ApiResponse<Status>> UpdateStatusAsync(int id, Status status)
        {
            var existingStatus = await _context.Statuses.FindAsync(id);
            if (existingStatus == null || existingStatus.DeletedAt != null)
            {
                return new ApiResponse<Status>
                {
                    Success = false,
                    Message = "Status not found or already deleted.",
                    Data = null
                };
            }

            existingStatus.StatusName = status.StatusName;
            existingStatus.IsDeleted = status.IsDeleted;
            existingStatus.UpdatedAt = DateTime.Now;
            existingStatus.UpdatedBy = status.UpdatedBy;

            _context.Statuses.Update(existingStatus);
            await _context.SaveChangesAsync();

            return new ApiResponse<Status>
            {
                Success = true,
                Message = "Status updated successfully.",
                Data = existingStatus
            };
        }

        public async Task<ApiResponse<bool>> DeleteStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null || status.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Status not found or already deleted.",
                    Data = false
                };
            }

            status.DeletedAt = DateTime.Now;
            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Status deleted successfully.",
                Data = true
            };
        }
    }
}



