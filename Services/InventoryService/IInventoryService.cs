using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Helper;  // Assuming ApiResponse is defined here

namespace Pacifica.API.Services.InventoryService
{
    public interface IInventoryService
    {
        // Create a new Weekly Inventory
        Task<ApiResponse<ResponseInventoryDTO>> CreateInventoryAsync(CreateInventoryDTO inventoryDto);

        // Update an existing Weekly Inventory
        Task<ApiResponse<ResponseInventoryDTO>> UpdateInventoryAsync(int id, UpdateInventoryDTO inventoryDto);

        // Get Weekly Inventories for a branch in a date range
        Task<ApiResponse<IEnumerable<ResponseInventoryDTO>>> GetInventoriesAsync(int branchId, DateTime startDate, DateTime endDate);

        // Get Weekly Inventories filtered by BranchId, ProductId, Month, and WeekNumber
        Task<ApiResponse<IEnumerable<ResponseInventoryDTO>>> GetFilteredInventoriesAsync(FilterInventoryParams filterParams);

        // Get a specific Weekly Inventory by ID
        Task<ApiResponse<ResponseInventoryDTO>> GetInventoryByIdAsync(int id);

        // Calculate discrepancy (positive or negative)
        Task<ApiResponse<int>> CalculateDiscrepancyAsync(int systemQuantity, int actualQuantity);

        Task<ApiResponse<IEnumerable<ResponseViewInventoryDTO>>> GetViewInventoriesAsync(ViewInventoryParams filterParams);

        Task<ApiResponse<IEnumerable<WI_ResponseSearchBranchProduct>>> GetFilteredBranchProductAsync(WI_BranchProductSearchParams filterParams);

    }
}
