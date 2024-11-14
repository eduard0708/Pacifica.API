namespace Pacifica.API.Services.ProductStatusService
{
    public interface IProductStatusService
    {
        Task<ApiResponse<IEnumerable<ProductStatus>>> GetAllProductStatusesAsync();
        Task<ApiResponse<ProductStatus>> GetProductStatusByIdAsync(int id);
        Task<ApiResponse<ProductStatus>> CreateProductStatusAsync(ProductStatus productStatus);
        Task<ApiResponse<ProductStatus>> UpdateProductStatusAsync(int id, ProductStatus productStatus);
        Task<ApiResponse<bool>> DeleteProductStatusAsync(int id);
    }
}