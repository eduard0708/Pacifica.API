using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        // Existing methods...
        Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<BranchProductResponseDto>> GetProductsInBranchAsync(int branchId, int productId);
        Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> GetBranchProductsByPageAsync(
               int branchId,
               int page,
               int pageSize,
               string sortField,
               int sortOrder);
        Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts);
        Task<ApiResponse<IEnumerable<GetBranchProductFilterDto>>> GetFilteredBranchProductsAsync(FilterBranchProductsParams filter);
        Task<ApiResponse<BranchProductResponseDto>> UpdateBranchProductAsync(UpdateBranchProductDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteBranchProductAsync(SoftDeleteBranchProductParams deleteBranchProduct);
        Task<ApiResponse<List<BranchProductAuditTrailsDto>>> GetBranchProductAuditTrailsAsync(int branchId, int productId);
        Task<ApiResponse<IEnumerable<BranchProductDto>>> GetAllDeletedBranchProductsAsync();
        Task<ApiResponse<List<int>>> RestoreDeletedBrachProductsAsync(RestoreDeletedBranchProductsParams restoreDeleted);
        Task<ApiResponse<IEnumerable<BranchProductForStockInDTO>>> GetBranchProductsByCategoryAndSupplierAsync(int branchId, int categoryId, int supplierId);

        // New method for searching by SKU
        Task<ApiResponse<IEnumerable<BranchProductForStockInDTO>>> GetBranchProductsBySKUAsync(int branchId, string sku);

    }
}
