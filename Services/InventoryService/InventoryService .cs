using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Models.Inventory;

namespace Pacifica.API.Services.InventoryService
{
    public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InventoryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BeginningInventoryDto> SetBeginningInventoryAsync(BeginningInventoryDto dto)
    {
        var beginningInventory = _mapper.Map<BeginningInventory>(dto);
        _context.BeginningInventories.Add(beginningInventory);
        await _context.SaveChangesAsync();

        return _mapper.Map<BeginningInventoryDto>(beginningInventory);
    }

    public async Task<MonthlyInventoryDto> UpdateMonthlyInventoryAsync(MonthlyInventoryDto dto)
    {
        var monthlyInventory = _mapper.Map<MonthlyInventory>(dto);
        _context.MonthlyInventories.Add(monthlyInventory);
        await _context.SaveChangesAsync();

        // Record audit trail
        var auditTrailDto = new BranchProductInventoryAuditTrailDto
        {
            BranchId = dto.BranchId,
            ProductId = dto.ProductId,
            PreviousStockQuantity = dto.StockQuantity,  // Previous stock before update
            AdjustedQuantity = dto.StockQuantity,
            AdjustedBy = "System",
            DateAdjusted = DateTime.UtcNow,
            AdjustmentReason = "Monthly Update"
        };

        await RecordAuditTrailAsync(auditTrailDto);

        return _mapper.Map<MonthlyInventoryDto>(monthlyInventory);
    }

    public async Task<BranchProductInventoryAuditTrailDto> RecordAuditTrailAsync(BranchProductInventoryAuditTrailDto dto)
    {
        var auditTrail = _mapper.Map<BranchProductInventoryAuditTrail>(dto);
        _context.BranchProductInventoryAuditTrails.Add(auditTrail);
        await _context.SaveChangesAsync();

        return _mapper.Map<BranchProductInventoryAuditTrailDto>(auditTrail);
    }
}

}