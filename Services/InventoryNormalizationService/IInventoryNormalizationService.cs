
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Models.Inventory;

namespace Pacifica.API.Services.InventoryNormalizationService
{

    public interface IInventoryNormalizationService
    {
        Task<IEnumerable<InventoryNormalization>> GetAllAsync();
        Task<InventoryNormalization?> GetByIdAsync(int id);
        Task<InventoryNormalization> CreateAsync(CreateInventoryNormalizationDto dto);
        Task<InventoryNormalization?> UpdateAsync(int id, InventoryNormalizationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ApiResponse<IEnumerable<ResponseNormalizeProduct>>> ViewNormalizationAsyc(InventoryNormalizeParams filterParams);

    }
}

