
namespace Pacifica.API.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<ApiResponse<IEnumerable<Category>>> GetCategoriesByPageAsync(int page, int pageSize, string sortField, int sortOrder);

        Task<ApiResponse<Category>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<Category>> CreateCategoryAsync(Category category);
        Task<ApiResponse<Category>> UpdateCategoryAsync(int id, Category category);
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);
    }
}