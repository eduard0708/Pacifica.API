using Pacifica.API.Dtos.BranchProduct;
namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<BranchProductResponseDto>> AddProductToBranchAsync(BranchProduct branchProduct);
        Task<ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>> GetProductsFilteredByBranchAsync(int branchId, string? productCategory = null, string? sku = null);
    }
}
