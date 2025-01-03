using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Dtos.Category;

namespace Pacifica.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.DeletedAt == null) // Ensure soft delete is respected
                .ToListAsync();

            if (!categories.Any())
            {
                return new ApiResponse<IEnumerable<Category>>
                {
                    Success = false,
                    Message = "No categories found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<Category>>
            {
                Success = true,
                Message = "Categories retrieved successfully.",
                Data = categories
            };
        }

        public async Task<ApiResponse<IEnumerable<SelectCategoryDTO>>> GetSelectCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.DeletedAt == null) // Ensure soft delete is respected
                .Select(c => new SelectCategoryDTO
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();

            if (!categories.Any())
            {
                return new ApiResponse<IEnumerable<SelectCategoryDTO>>
                {
                    Success = false,
                    Message = "No categories found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<SelectCategoryDTO>>
            {
                Success = true,
                Message = "Categories retrieved successfully.",
                Data = categories
            };
        }


        public async Task<ApiResponse<IEnumerable<Category>>> GetCategoriesByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {

            // Map sortField to an actual Expression<Func<Branch, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<Category>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            var totalCount = await _context.Categories
                .IgnoreQueryFilters() // Ignore QueryFilters for soft delete    
                .CountAsync();

            // Dynamically order the query based on the sort expression and sort order
            IQueryable<Category> query = _context.Categories
                 .IgnoreQueryFilters();  // Ignore global filters, so we can apply soft delete filter manually

            // Apply sorting dynamically based on sortOrder
            query = sortOrder == 1 ? _context.Categories.IgnoreQueryFilters().OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

            // Apply pagination
            var branches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<Category>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = branches,
                TotalCount = totalCount
            };
        }

        private Expression<Func<Category, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "categoryName":
                    return x => x.CategoryName!;
                case "description":
                    return x => x.Description!;
                case "createdAt":
                    return x => x.CreatedAt;
                case "isDeleted":
                    return x => x.IsDeleted!;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<IEnumerable<Category>>> GetBranchesByPageAsync(int page, int pageSize)
        {
            var totalCount = await _context.Categories
                .Where(b => b.DeletedAt == null) // Soft delete filter
                .CountAsync();

            var branches = await _context.Categories
                .Where(b => b.DeletedAt == null) // Soft delete filter
                .Skip((page - 1) * pageSize)  // Skip based on page number and page size
                .Take(pageSize)               // Take the number of items specified by pageSize
                .ToListAsync();

            if (!branches.Any())
            {
                return new ApiResponse<IEnumerable<Category>>
                {
                    Success = false,
                    Message = "No branches found.",
                    Data = null,
                    TotalCount = totalCount // Include the total count for pagination
                };
            }

            return new ApiResponse<IEnumerable<Category>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = branches,
                TotalCount = totalCount // Include the total count for pagination
            };
        }

        public async Task<ApiResponse<Category>> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (category == null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category not found.",
                    Data = null
                };
            }

            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Category retrieved successfully.",
                Data = category
            };
        }

        public async Task<ApiResponse<Category>> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Category created successfully.",
                Data = category
            };
        }

        public async Task<ApiResponse<Category>> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null || existingCategory.DeletedAt != null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category not found or already deleted.",
                    Data = null
                };
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            existingCategory.UpdatedAt = DateTime.Now;
            existingCategory.UpdatedBy = category.UpdatedBy;
            existingCategory.IsDeleted = true;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Category updated successfully.",
                Data = existingCategory
            };
        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Category not found or already deleted.",
                    Data = false
                };
            }

            category.DeletedAt = DateTime.Now;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Category deleted successfully.",
                Data = true
            };
        }
    }
}
