using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.BranchProductService
{
    public interface IBranchProductService
    {
        Task<ApiResponse<IEnumerable<BranchProduct>>> GetAllProductsByBranchAsync(int branchId);
        Task<ApiResponse<BranchProduct>> AddProductToBranchAsync(BranchProduct branchProduct);
    }
}