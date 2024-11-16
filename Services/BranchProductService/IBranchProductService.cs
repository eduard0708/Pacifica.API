using Pacifica.API.Dtos.BranchProduct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        // Existing methods...
        Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts); 
        Task<ApiResponse<IEnumerable<GetBranchProductFilterDto>>> GetProductsFilteredByBranchAsync(int branchId, string? productCategory = null, string? sku = null, string? productName = null);
        
        // New methods
        Task<ApiResponse<BranchProductResponseDto>> UpdateBranchProductAsync(int branchProductId, UpdateBranchProductDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteBranchProductAsync(int branchProductId);
    }
}
