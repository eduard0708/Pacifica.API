using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<BranchProductResponseDto>> AddProductToBranchAsync(BranchProduct branchProduct);
    }
}