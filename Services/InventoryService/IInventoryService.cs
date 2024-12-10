using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Helper;  // Assuming ApiResponse is defined here

namespace Pacifica.API.Services.InventoryService
{
    public interface IInventoryService
    {
        // Create a new Weekly Inventory
        Task<ApiResponse<ResponseWeeklyInventoryDTO>> CreateWeeklyInventoryAsync(CreateWeeklyInventoryDTO inventoryDto);

        // Update an existing Weekly Inventory
        Task<ApiResponse<ResponseWeeklyInventoryDTO>> UpdateWeeklyInventoryAsync(int id, UpdateWeeklyInventoryDTO inventoryDto);

        // Get Weekly Inventories for a branch in a date range
        Task<ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>> GetWeeklyInventoriesAsync(int branchId, DateTime startDate, DateTime endDate);

        // Get Weekly Inventories filtered by BranchId, ProductId, Month, and WeekNumber
        Task<ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>> GetFilteredWeeklyInventoriesAsync(FilterWeeklyInventoryParams filterParams);

        // Get a specific Weekly Inventory by ID
        Task<ApiResponse<ResponseWeeklyInventoryDTO>> GetWeeklyInventoryByIdAsync(int id);

        // Calculate discrepancy (positive or negative)
        Task<ApiResponse<int>> CalculateDiscrepancyAsync(int systemQuantity, int actualQuantity);

        Task<ApiResponse<IEnumerable<ResponseViewInventoryDTO>>> GetViewInventoriesAsync(ViewInventoryParams filterParams);

    }
}
