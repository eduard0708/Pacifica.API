using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        // Existing methods...
        Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts);
        Task<ApiResponse<IEnumerable<GetBranchProductFilterDto>>> GetFilteredBranchProductsAsync(FilterBranchProductsParams filter);
        Task<ApiResponse<BranchProductResponseDto>> UpdateBranchProductAsync(UpdateBranchProductDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteBranchProductAsync(SoftDeleteBranchProductParams deleteBranchProduct);
        Task<ApiResponse<List<BranchProductAuditTrailsDto>>> GetBranchProductAuditTrailsAsync(int branchId, int productId);
        Task<ApiResponse<IEnumerable<BranchProductDto>>> GetAllDeletedBranchProductsAsync();
        Task<ApiResponse<List<int>>> RestoreDeletedBrachProductsAsync(RestoreDeletedBranchProductsParams restoreDeleted);
    }
}
