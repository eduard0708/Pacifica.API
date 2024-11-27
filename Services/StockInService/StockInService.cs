using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Services.StockInService
{
    public class StockInService : IStockInService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockInService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all StockIns
        public async Task<ApiResponse<IEnumerable<StockInDTO>>> GetAllStockInsAsync()
        {
            var stockIns = await _context.StockIns
                .Where(si => si.DeletedAt == null)  // Only not-deleted StockIns
                .ToListAsync();

            if (!stockIns.Any())
            {
                return new ApiResponse<IEnumerable<StockInDTO>>
                {
                    Success = false,
                    Message = "No StockIn records found.",
                    Data = null
                };
            }

            var stockInDtos = _mapper.Map<IEnumerable<StockInDTO>>(stockIns);

            return new ApiResponse<IEnumerable<StockInDTO>>
            {
                Success = true,
                Message = "StockIn records retrieved successfully.",
                Data = stockInDtos
            };
        }

        // Get StockIn by Id
        public async Task<ApiResponse<StockInDTO>> GetStockInByIdAsync(int id)
        {
            var stockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.Id == id && si.DeletedAt == null);

            if (stockIn == null)
            {
                return new ApiResponse<StockInDTO>
                {
                    Success = false,
                    Message = "StockIn record not found.",
                    Data = null
                };
            }

            var stockInDto = _mapper.Map<StockInDTO>(stockIn);

            return new ApiResponse<StockInDTO>
            {
                Success = true,
                Message = "StockIn record retrieved successfully.",
                Data = stockInDto
            };
        }

        // Create a new StockIn (single record)
        public async Task<ApiResponse<StockInDTO>> CreateStockInAsync(StockInCreateDTO stockInDto)
        {
            var stockIn = _mapper.Map<StockIn>(stockInDto);

            // Add the StockIn record
            _context.StockIns.Add(stockIn);
            await _context.SaveChangesAsync();

            // Update the BranchProduct stock quantity
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == stockInDto.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                branchProduct.StockQuantity += stockInDto.Quantity; // Add the quantity to the existing stock
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();
            }

            var createdStockInDto = _mapper.Map<StockInDTO>(stockIn);

            return new ApiResponse<StockInDTO>
            {
                Success = true,
                Message = "StockIn record created successfully.",
                Data = createdStockInDto
            };
        }

        // Create multiple StockIn records (bulk creation)
        public async Task<ApiResponse<IEnumerable<StockInDTO>>> CreateMultipleStockInAsync(IEnumerable<StockInCreateDTO> stockInDtos)
        {
            var stockIns = _mapper.Map<IEnumerable<StockIn>>(stockInDtos);

            _context.StockIns.AddRange(stockIns);
            await _context.SaveChangesAsync();

            // Update BranchProduct stock quantities for each StockIn
            foreach (var stockInDto in stockInDtos)
            {
                var branchProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == stockInDto.ProductId && bp.DeletedAt == null);

                if (branchProduct != null)
                {
                    branchProduct.StockQuantity += stockInDto.Quantity; // Add the quantity to the existing stock
                    _context.BranchProducts.Update(branchProduct);
                }
            }

            await _context.SaveChangesAsync();

            var createdStockInDtos = _mapper.Map<IEnumerable<StockInDTO>>(stockIns);

            return new ApiResponse<IEnumerable<StockInDTO>>
            {
                Success = true,
                Message = $"{createdStockInDtos.Count()} StockIn records created successfully.",
                Data = createdStockInDtos
            };
        }

        // Update existing StockIn
        public async Task<ApiResponse<StockInDTO>> UpdateStockInAsync(int id, StockInUpdateDTO stockInDto)
        {
            var existingStockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.Id == id && si.DeletedAt == null);

            if (existingStockIn == null)
            {
                return new ApiResponse<StockInDTO>
                {
                    Success = false,
                    Message = "StockIn record not found.",
                    Data = null
                };
            }

            // Store the old values for audit trail
            var oldProductId = existingStockIn.ProductId;
            var oldQuantity = existingStockIn.Quantity;
            var oldBranchId = existingStockIn.BranchId;
            var oldCostPrice = existingStockIn.CostPrice;
            var oldReferenceNumber = existingStockIn.ReferenceNumber;
            var oldStockInReferenceId = existingStockIn.StockInReferenceId;

            // Store the old values for the audit trail
            var oldValues = $"ProductId: {existingStockIn.ProductId}, Quantity: {existingStockIn.Quantity}, BranchId: {existingStockIn.BranchId}, CostPrice: {existingStockIn.CostPrice}, ReferenceNumber: {existingStockIn.ReferenceNumber},  StockInReferenceId: {existingStockIn.StockInReferenceId}";

            // Map the updated values to the existing entity
            _mapper.Map(stockInDto, existingStockIn);
            existingStockIn.UpdatedAt = DateTime.Now;  // Update timestamp
            existingStockIn.UpdatedBy = stockInDto.UpdatedBy;  // Assuming CreatedBy is passed in DTO

            _context.StockIns.Update(existingStockIn);
            await _context.SaveChangesAsync();

            // Create the audit trail entry
            var stockInAuditTrail = new StockInAuditTrail
            {
                StockInId = existingStockIn.Id,
                Action = "Updated",
                OldValue = oldValues,
                NewValue = $"ProductId: {stockInDto.ProductId}, Quantity: {stockInDto.Quantity}, BranchId: {stockInDto.BranchId}, CostPrice: {stockInDto.CostPrice},ReferenceNumber: {existingStockIn.ReferenceNumber}, StockInReferenceId: {existingStockIn.StockInReferenceId}",
                ActionBy = stockInDto.UpdatedBy,
                Remarks = stockInDto.Remarks,
                ActionDate = DateTime.Now
            };

            _context.StockInAuditTrails.Add(stockInAuditTrail);
            await _context.SaveChangesAsync();

            // Proceed with updating the BranchProduct (if needed)
            var oldBranchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == oldProductId && bp.DeletedAt == null);

            if (oldBranchProduct != null)
            {
                oldBranchProduct.StockQuantity -= oldQuantity; // Deduct old quantity
                _context.BranchProducts.Update(oldBranchProduct);
            }

            var newBranchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == stockInDto.ProductId && bp.DeletedAt == null);

            if (newBranchProduct != null)
            {
                newBranchProduct.StockQuantity += stockInDto.Quantity; // Add new quantity
                _context.BranchProducts.Update(newBranchProduct);
            }

            await _context.SaveChangesAsync();

            var updatedStockInDto = _mapper.Map<StockInDTO>(existingStockIn);

            return new ApiResponse<StockInDTO>
            {
                Success = true,
                Message = "StockIn record updated successfully.",
                Data = updatedStockInDto
            };
        }

        // Get StockIn by Reference Number
        public async Task<ApiResponse<IEnumerable<StockInDTO>>> GetStockInByReferenceNumberAsync(string referenceNumber)
        {
            // Fetch all stock-in records with the given reference number
            var stockIns = await _context.StockIns
                .Where(si => si.ReferenceNumber == referenceNumber && si.DeletedAt == null) // Apply any necessary filters, e.g., DeletedAt check
                .ToListAsync();

            if (stockIns == null || !stockIns.Any())
            {
                return new ApiResponse<IEnumerable<StockInDTO>>
                {
                    Success = false,
                    Message = "No StockIn records found with the given reference number.",
                    Data = null
                };
            }

            // Map the stock-in entities to DTOs
            var stockInDtos = _mapper.Map<IEnumerable<StockInDTO>>(stockIns);

            return new ApiResponse<IEnumerable<StockInDTO>>
            {
                Success = true,
                Message = "StockIn records retrieved successfully.",
                Data = stockInDtos
            };
        }

        // // Delete StockIn (soft delete by setting DeletedAt)
        public async Task<ApiResponse<bool>> DeleteStockInAsync(StockInDeleteParams deleteParams)
        {
            // Fetch the StockIn record by ID and check if it's not deleted already.
            var stockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.Id == deleteParams.Id && si.DeletedAt == null);

            if (stockIn == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "StockIn record not found.",
                    Data = false
                };
            }

            // Save the old values for the audit trail (before deletion).
            var oldValues = $"ProductId: {stockIn.ProductId}, Quantity: {stockIn.Quantity}, BranchId: {stockIn.BranchId}";

            // Perform soft delete (set DeletedAt and DeletedBy).
            stockIn.DeletedAt = DateTime.Now;
            stockIn.DeletedBy = deleteParams.ActionBy;  // Should be the authenticated user.

            _context.StockIns.Update(stockIn);
            await _context.SaveChangesAsync();

            // Create the audit trail entry for deletion of StockIn.
            var stockInAuditTrail = new StockInAuditTrail
            {
                StockInId = stockIn.Id,
                Action = "Deleted",
                OldValue = oldValues,
                NewValue = null, // Nothing to show for deleted record
                Remarks = deleteParams.Remarks,  // User can add a remark for the deletion
                ActionBy = deleteParams.ActionBy,  // This should be dynamic, based on the current user
                ActionDate = DateTime.Now
            };

            _context.StockInAuditTrails.Add(stockInAuditTrail);
            await _context.SaveChangesAsync();

            // Update the BranchProduct quantity (remove stock as it's been deleted).
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockIn.BranchId && bp.ProductId == stockIn.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                // Save the old quantity for audit trail before updating
                var branchProductOldQuantity = branchProduct.StockQuantity;

                // Decrease the stock quantity in BranchProduct by the quantity in StockIn
                branchProduct.StockQuantity -= stockIn.Quantity;

                // If the stock quantity becomes negative, you might want to set it to 0 or throw an error
                if (branchProduct.StockQuantity < 0)
                {
                    branchProduct.StockQuantity = 0; // Optionally reset to 0 if stock cannot go negative
                }

                // Update BranchProduct stock quantity
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();

                // Create the audit trail entry for BranchProduct update
                var branchProductAuditTrail = new BranchProductAuditTrail
                {
                    BranchId = branchProduct.BranchId,
                    ProductId = branchProduct.ProductId,
                    Action = "StockIn Deleted",  // Action description
                    OldValue = $"Quantity: {branchProductOldQuantity}",  // Old quantity before deletion
                    NewValue = $"Quantity: {branchProduct.StockQuantity}",  // New quantity after deletion
                    ActionBy = deleteParams.ActionBy,
                    Remarks = deleteParams.Remarks,  // User performing the action
                    ActionDate = DateTime.Now
                };

                // Add the audit trail for the branch product update
                _context.BranchProductAuditTrails.Add(branchProductAuditTrail);
                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "StockIn record deleted successfully and BranchProduct stock updated.",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> RestoreStockInAsync(StockInRestoreParams restoreParams)
        {
            // Fetch the StockIn record by ID and check if it's marked as deleted.
            var stockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.Id == restoreParams.Id && si.DeletedAt != null);

            if (stockIn == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "StockIn record not found or is not deleted.",
                    Data = false
                };
            }

            // Save the old values for audit trail (before restore).
            var oldValues = $"ProductId: {stockIn.ProductId}, Quantity: {stockIn.Quantity}, BranchId: {stockIn.BranchId}";

            // Restore the StockIn record by setting DeletedAt back to null.
            stockIn.DeletedAt = null;
            stockIn.DeletedBy = null;  // Optionally reset DeletedBy if needed.

            _context.StockIns.Update(stockIn);
            await _context.SaveChangesAsync();

            // Create the audit trail entry for the restore action.
            var stockInAuditTrail = new StockInAuditTrail
            {
                StockInId = stockIn.Id,
                Action = "Restored",
                OldValue = null, // Nothing to show before it was deleted
                NewValue = oldValues, // Show the old values after restoration
                Remarks = restoreParams.Remarks,
                ActionBy = restoreParams.ActionBy,  // This should be dynamic, based on the current user
                ActionDate = DateTime.Now
            };

            _context.StockInAuditTrails.Add(stockInAuditTrail);
            await _context.SaveChangesAsync();

            // Optionally, you can update the BranchProduct stock quantity when restoring.
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockIn.BranchId && bp.ProductId == stockIn.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                // Save the old quantity for the audit trail before updating
                var branchProductOldQuantity = branchProduct.StockQuantity;

                // Increase the stock quantity in BranchProduct by the quantity of the restored StockIn
                branchProduct.StockQuantity += stockIn.Quantity;

                // Update BranchProduct stock quantity
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();

                // Create the audit trail entry for BranchProduct update (stock restored)
                var branchProductAuditTrail = new BranchProductAuditTrail
                {
                    BranchId = branchProduct.BranchId,
                    ProductId = branchProduct.ProductId,
                    Action = "Deleted StockIn Restored",  // Action description
                    OldValue = $"Quantity: {branchProductOldQuantity}",  // Old quantity before restore
                    NewValue = $"Quantity: {branchProduct.StockQuantity}",  // New quantity after restore
                    Remarks = restoreParams.Remarks,
                    ActionBy = restoreParams.ActionBy,  // User performing the restore action
                    ActionDate = DateTime.Now
                };

                // Add the audit trail for the branch product update
                _context.BranchProductAuditTrails.Add(branchProductAuditTrail);
                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "StockIn record restored successfully and BranchProduct stock updated.",
                Data = true
            };
        }

        public async Task<ApiResponse<List<StockIn>>> GetAllDeletedStockInAsync()
        {
            // Fetch all soft-deleted records (where DeletedAt is not null)
            var deletedStockIn = await _context.StockIns
                .Where(si => si.DeletedAt != null)  // Filter records where DeletedAt is not null
                .ToListAsync();

            if (deletedStockIn == null || deletedStockIn.Count == 0)
            {
                return new ApiResponse<List<StockIn>>
                {
                    Success = false,
                    Message = "No deleted StockIn records found.",
                    Data = null
                };
            }

            return new ApiResponse<List<StockIn>>
            {
                Success = true,
                Message = "Deleted StockIn records retrieved successfully.",
                Data = deletedStockIn
            };
        }

    }
}
