using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.BranchService
{
    public interface IBranchService
    {
        Task<ApiResponse<IEnumerable<Branch>>> GetAllBranchesAsync();
        Task<ApiResponse<Branch>> GetBranchByIdAsync(int id);
        Task<ApiResponse<Branch>> CreateBranchAsync(Branch branch);
        Task<ApiResponse<Branch>> UpdateBranchAsync(int id, Branch branch);
        Task<ApiResponse<bool>> DeleteBranchAsync(int id);
    }
}