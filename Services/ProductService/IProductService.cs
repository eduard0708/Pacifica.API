using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;

namespace Pacifica.API.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ApiResponse<IEnumerable<Product>>> GetAllDeletedProductsAsync();
        Task<ApiResponse<Product>> GetProductByIdAsync(int id);
        Task<ApiResponse<Product>> CreateProductAsync(Product product);
        Task<ApiResponse<Product>> UpdateProductAsync(int id, Product product);
        Task<ApiResponse<bool>> DeleteProductsAsync(ToDeletedProductsParam productsDelete);
        Task<ApiResponse<IEnumerable<Product>>> RestoreDeletedProductsAsync(RestoreDeletedProductsParam deletedProducts);
        Task<ApiResponse<List<AuditDetails>>> GetProductAuditDetailsAsync(int productId);
        Task<ApiResponse<IEnumerable<GetFilter_Products>>> GetFilterProductsAsync(string? category = null, string? sku = null, string? productName = null);
    }
}