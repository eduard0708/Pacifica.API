namespace Pacifica.API.Services.BranchService
{
    public interface IBranchService
    {
        Task<ApiResponse<IEnumerable<Branch>>> GetAllBranchesAsync();
        Task<ApiResponse<IEnumerable<Branch>>> GetBranchesByPageAsync(int page, int pageSize, string sortField, int sortOrder);
        Task<ApiResponse<Branch>> GetBranchByIdAsync(int id);
        Task<ApiResponse<Branch>> CreateBranchAsync(Branch branch);
        Task<ApiResponse<Branch>> UpdateBranchAsync(int id, Branch branch);
        Task<ApiResponse<bool>> DeleteBranchAsync(int id);
    }
}
