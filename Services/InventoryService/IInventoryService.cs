using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pacifica.API.Dtos.Inventory;

namespace Pacifica.API.Services.InventoryService
{
    public interface IInventoryService
    {
        Task<BeginningInventoryDto> SetBeginningInventoryAsync(BeginningInventoryDto dto);
        Task<MonthlyInventoryDto> UpdateMonthlyInventoryAsync(MonthlyInventoryDto dto);
        Task<BranchProductInventoryAuditTrailDto> RecordAuditTrailAsync(BranchProductInventoryAuditTrailDto dto);
    }
}