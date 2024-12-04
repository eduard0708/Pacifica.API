using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;

namespace Pacifica.API.Services.BranchService
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BranchService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<Branch>>> GetAllBranchesAsync()
        {
            var branches = await _context.Branches
                .Where(b => b.DeletedAt == null) // Soft delete filter
                .ToListAsync();

            if (!branches.Any())
            {
                return new ApiResponse<IEnumerable<Branch>>
                {
                    Success = false,
                    Message = "No branches found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<Branch>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = branches
            };
        }

        public async Task<ApiResponse<IEnumerable<Branch>>> GetBranchesByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {
            var validSortFields = new List<string> { "branchName", "branchLocation", "createdAt" };

            if (!validSortFields.Contains(sortField))
            {
                return new ApiResponse<IEnumerable<Branch>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                };
            }

            // Map sortField to an actual Expression<Func<Branch, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<Branch>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            var totalCount = await _context.Branches
                .Where(b => b.DeletedAt == null) // Soft delete filter
                .CountAsync();

            // Dynamically order the query based on the sort expression and sort order
            IQueryable<Branch> query = _context.Branches
                .Where(b => b.DeletedAt == null); // Soft delete filter

            // Apply sorting dynamically based on sortOrder
            query = sortOrder == 1 ? query.OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

            // Apply pagination
            var branches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<Branch>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = branches,
                TotalCount = totalCount
            };
        }

        private Expression<Func<Branch, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "branchName":
                    return x => x.BranchName;
                case "branchLocation":
                    return x => x.BranchLocation;
                case "createdAt":
                    return x => x.CreatedAt;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<Branch>> GetBranchByIdAsync(int id)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

            if (branch == null)
            {
                return new ApiResponse<Branch>
                {
                    Success = false,
                    Message = "Branch not found.",
                    Data = null
                };
            }

            return new ApiResponse<Branch>
            {
                Success = true,
                Message = "Branch retrieved successfully.",
                Data = branch
            };
        }

        public async Task<ApiResponse<Branch>> CreateBranchAsync(Branch branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return new ApiResponse<Branch>
            {
                Success = true,
                Message = "Branch created successfully.",
                Data = branch
            };
        }

        public async Task<ApiResponse<Branch>> UpdateBranchAsync(int id, Branch branch)
        {
            var existingBranch = await _context.Branches.FindAsync(id);
            if (existingBranch == null || existingBranch.DeletedAt != null)
            {
                return new ApiResponse<Branch>
                {
                    Success = false,
                    Message = "Branch not found or already deleted.",
                    Data = null
                };
            }

            existingBranch.BranchName = branch.BranchName;
            existingBranch.BranchLocation = branch.BranchLocation;
            existingBranch.IsDeleted = branch.IsDeleted;
            existingBranch.UpdatedAt = DateTime.Now;
            existingBranch.UpdatedBy = branch.UpdatedBy;

            _context.Branches.Update(existingBranch);
            await _context.SaveChangesAsync();

            return new ApiResponse<Branch>
            {
                Success = true,
                Message = "Branch updated successfully.",
                Data = existingBranch
            };
        }

        public async Task<ApiResponse<bool>> DeleteBranchAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null || branch.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Branch not found or already deleted.",
                    Data = false
                };
            }

            branch.DeletedAt = DateTime.Now;
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Branch deleted successfully.",
                Data = true
            };
        }
    }
}
