using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Helper;

namespace Pacifica.API.Services.BeginningInventoryService
{
    public interface IBeginningInventoryService
    {
        // Create a new BeginningInventory
        Task<ApiResponse<BeginningInventoryDto>> CreateAsync(CreateBeginningInventoryDto model);

        // Get a BeginningInventory by its Id
        Task<ApiResponse<BeginningInventoryDto>> GetByIdAsync(int id);

        // Get all BeginningInventories with pagination and sorting
        Task<ApiResponse<List<BeginningInventoryDto>>> GetAllLazyAsync(int page, int size, string sortField, int sortOrder);

        // Update an existing BeginningInventory
        Task<ApiResponse<BeginningInventoryDto>> UpdateAsync(int id, UpdateBeginningInventoryDto model);

        // Get BeginningInventories by BranchId
        Task<ApiResponse<List<BeginningInventoryDto>>> GetByBranchIdAsync(int branchId);


        // Delete a BeginningInventory by its Id
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
