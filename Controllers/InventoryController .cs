
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Services.InventoryService;

namespace Pacifica.API.Controllers
{
   [Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // POST: api/Inventory/SetBeginningInventory
    [HttpPost("SetBeginningInventory")]
    public async Task<ActionResult<BeginningInventoryDto>> SetBeginningInventory(BeginningInventoryDto dto)
    {
        var result = await _inventoryService.SetBeginningInventoryAsync(dto);
        return CreatedAtAction(nameof(SetBeginningInventory), new { id = result.Id }, result);
    }

    // POST: api/Inventory/UpdateMonthlyInventory
    [HttpPost("UpdateMonthlyInventory")]
    public async Task<ActionResult<MonthlyInventoryDto>> UpdateMonthlyInventory(MonthlyInventoryDto dto)
    {
        var result = await _inventoryService.UpdateMonthlyInventoryAsync(dto);
        return CreatedAtAction(nameof(UpdateMonthlyInventory), new { id = result.BranchId }, result);
    }

    // POST: api/Inventory/RecordAuditTrail
    [HttpPost("RecordAuditTrail")]
    public async Task<ActionResult<BranchProductInventoryAuditTrailDto>> RecordAuditTrail(BranchProductInventoryAuditTrailDto dto)
    {
        var result = await _inventoryService.RecordAuditTrailAsync(dto);
        return CreatedAtAction(nameof(RecordAuditTrail), new { id = result.Id }, result);
    }
}

}