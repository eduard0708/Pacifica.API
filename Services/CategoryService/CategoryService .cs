using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;

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
