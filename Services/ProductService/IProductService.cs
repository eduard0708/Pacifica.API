using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ApiResponse<Product>> GetProductByIdAsync(int id);
        Task<ApiResponse<Product>> CreateProductAsync(Product product);
        Task<ApiResponse<Product>> UpdateProductAsync(int id, Product product);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<IEnumerable<GetFilter_Products>>> GetFilterProductsAsync(string? category = null, string? sku = null, string? productStatus = null, string? productName = null);
    }
}