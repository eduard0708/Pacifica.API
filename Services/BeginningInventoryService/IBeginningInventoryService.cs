using Pacifica.API.Dtos.Inventory;

namespace Pacifica.API.Services.BeginningInventoryService
{
    public interface IBeginningInventoryService
    {
        // Create a new BeginningInventory
        Task<BeginningInventoryDto> CreateAsync(CreateBeginningInventoryDto model);

        // Get a BeginningInventory by its Id
        Task<BeginningInventoryDto> GetByIdAsync(int id);

        // Get all BeginningInventories with pagination and sorting
        Task<ApiResponse<List<BeginningInventoryDto>>> GetAllAsync(int page, int size, string sortField, int sortOrder);

        // Update an existing BeginningInventory
        Task<BeginningInventoryDto> UpdateAsync(int id, UpdateBeginningInventoryDto model);

        // Delete a BeginningInventory by its Id
        Task<bool> DeleteAsync(int id);

    }
}