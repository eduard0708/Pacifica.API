using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;

namespace Pacifica.API.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponse<List<Product>>> GetAllProductsAsync();
        Task<ApiResponse<IEnumerable<Product>>> GetProductsByPageAsync(int page, int pageSize, string sortField, int sortOrder);
        Task<ApiResponse<IEnumerable<Product>>> GetAllDeletedProductsAsync();
        Task<ApiResponse<Product>> GetDeletedProductByIdAsync(int productId);
        Task<ApiResponse<Product>> GetProductByIdAsync(int id);
        Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto product);
        Task<ApiResponse<List<ProductDto>>> CreateMulipleProductsAsync(List<CreateProductDto> product);
        Task<ApiResponse<Product>> UpdateProductAsync(int id, Product product);
        Task<ApiResponse<bool>> DeleteProductsAsync(DeletedProductsParam productsDelete);
        Task<ApiResponse<IEnumerable<Product>>> RestoreDeletedProductsAsync(RestoreDeletedProductsParam deletedProducts);
        Task<ApiResponse<IEnumerable<GetFilter_Products>>> GetFilterProductsAsync(ProductFilterParams productFilter);
        Task<ApiResponse<List<AuditDetails>>> GetProductAuditDetailsAsync(int productId);
        Task<ApiResponse<List<ProductAuditTrailsDto>>> GetProductAuditTrailsAsync(int productId);
    }
}