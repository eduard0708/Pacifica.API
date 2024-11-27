using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockOut;
using Pacifica.API.Models.Transaction;
using Pacifica.API.Models.GlobalAuditTrails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.StockOutService
{
    public class StockOutService : IStockOutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockOutService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all StockOuts
        public async Task<ApiResponse<IEnumerable<StockOutDTO>>> GetAllStockOutsAsync()
        {
            var stockOuts = await _context.StockOuts
                .Where(so => so.DeletedAt == null) // Only not-deleted StockOuts
                .ToListAsync();

            if (!stockOuts.Any())
            {
                return new ApiResponse<IEnumerable<StockOutDTO>>
                {
                    Success = false,
                    Message = "No StockOut records found.",
                    Data = null
                };
            }

            var stockOutDtos = _mapper.Map<IEnumerable<StockOutDTO>>(stockOuts);

            return new ApiResponse<IEnumerable<StockOutDTO>>
            {
                Success = true,
                Message = "StockOut records retrieved successfully.",
                Data = stockOutDtos
            };
        }

        // Get StockOut by Id
        public async Task<ApiResponse<StockOutDTO>> GetStockOutByIdAsync(int id)
        {
            var stockOut = await _context.StockOuts
                .FirstOrDefaultAsync(so => so.Id == id && so.DeletedAt == null);

            if (stockOut == null)
            {
                return new ApiResponse<StockOutDTO>
                {
                    Success = false,
                    Message = "StockOut record not found.",
                    Data = null
                };
            }

            var stockOutDto = _mapper.Map<StockOutDTO>(stockOut);

            return new ApiResponse<StockOutDTO>
            {
                Success = true,
                Message = "StockOut record retrieved successfully.",
                Data = stockOutDto
            };
        }

        // Create a new StockOut (single record)
        public async Task<ApiResponse<StockOutDTO>> CreateStockOutAsync(StockOutCreateDTO stockOutDto)
        {
            var stockOut = _mapper.Map<StockOut>(stockOutDto);

            // Add the StockOut record
            _context.StockOuts.Add(stockOut);
            await _context.SaveChangesAsync();

            // Update the BranchProduct stock quantity
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockOutDto.BranchId && bp.ProductId == stockOutDto.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                branchProduct.StockQuantity -= stockOutDto.Quantity; // Subtract the quantity from the existing stock
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();
            }

            var createdStockOutDto = _mapper.Map<StockOutDTO>(stockOut);

            return new ApiResponse<StockOutDTO>
            {
                Success = true,
                Message = "StockOut record created successfully.",
                Data = createdStockOutDto
            };
        }

        // Create multiple StockOut records (bulk creation)
        public async Task<ApiResponse<IEnumerable<StockOutDTO>>> CreateMultipleStockOutAsync(IEnumerable<StockOutCreateDTO> stockOutDtos)
        {
            var stockOuts = _mapper.Map<IEnumerable<StockOut>>(stockOutDtos);

            _context.StockOuts.AddRange(stockOuts);
            await _context.SaveChangesAsync();

            // Update BranchProduct stock quantities for each StockOut
            foreach (var stockOutDto in stockOutDtos)
            {
                var branchProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == stockOutDto.BranchId && bp.ProductId == stockOutDto.ProductId && bp.DeletedAt == null);

                if (branchProduct != null)
                {
                    branchProduct.StockQuantity -= stockOutDto.Quantity; // Subtract the quantity from the existing stock
                    _context.BranchProducts.Update(branchProduct);
                }
            }

            await _context.SaveChangesAsync();

            var createdStockOutDtos = _mapper.Map<IEnumerable<StockOutDTO>>(stockOuts);

            return new ApiResponse<IEnumerable<StockOutDTO>>
            {
                Success = true,
                Message = $"{createdStockOutDtos.Count()} StockOut records created successfully.",
                Data = createdStockOutDtos
            };
        }

        // Update existing StockOut
        public async Task<ApiResponse<StockOutDTO>> UpdateStockOutAsync(int id, StockOutUpdateDTO stockOutDto)
        {
            var existingStockOut = await _context.StockOuts
                .FirstOrDefaultAsync(so => so.Id == id && so.DeletedAt == null);

            if (existingStockOut == null)
            {
                return new ApiResponse<StockOutDTO>
                {
                    Success = false,
                    Message = "StockOut record not found.",
                    Data = null
                };
            }

            // Store the old values for audit trail
            var oldProductId = existingStockOut.ProductId;
            var oldQuantity = existingStockOut.Quantity;
            var oldBranchId = existingStockOut.BranchId;

            // Store the old values for the audit trail
            var oldValues = $"ProductId: {existingStockOut.ProductId}, Quantity: {existingStockOut.Quantity}, BranchId: {existingStockOut.BranchId}";

            // Map the updated values to the existing entity
            _mapper.Map(stockOutDto, existingStockOut);
            existingStockOut.UpdatedAt = DateTime.Now;  // Update timestamp
            existingStockOut.UpdatedBy = stockOutDto.UpdatedBy;  // Assuming CreatedBy is passed in DTO

            _context.StockOuts.Update(existingStockOut);
            await _context.SaveChangesAsync();

            // Create the audit trail entry
            var stockOutAuditTrail = new StockOutAuditTrail
            {
                StockOutId = existingStockOut.Id,
                Action = "Updated",
                OldValue = oldValues,
                NewValue = $"ProductId: {stockOutDto.ProductId}, Quantity: {stockOutDto.Quantity}, BranchId: {stockOutDto.BranchId}",
                ActionBy = stockOutDto.UpdatedBy,
                Remarks = stockOutDto.Remarks,
                ActionDate = DateTime.Now
            };

            _context.StockOutAuditTrails.Add(stockOutAuditTrail);
            await _context.SaveChangesAsync();

            // Proceed with updating the BranchProduct (if needed)
            var oldBranchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockOutDto.BranchId && bp.ProductId == oldProductId && bp.DeletedAt == null);

            if (oldBranchProduct != null)
            {
                oldBranchProduct.StockQuantity += oldQuantity; // Add the old quantity back
                _context.BranchProducts.Update(oldBranchProduct);
            }

            var newBranchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockOutDto.BranchId && bp.ProductId == stockOutDto.ProductId && bp.DeletedAt == null);

            if (newBranchProduct != null)
            {
                newBranchProduct.StockQuantity -= stockOutDto.Quantity; // Subtract the new quantity
                _context.BranchProducts.Update(newBranchProduct);
            }

            await _context.SaveChangesAsync();

            var updatedStockOutDto = _mapper.Map<StockOutDTO>(existingStockOut);

            return new ApiResponse<StockOutDTO>
            {
                Success = true,
                Message = "StockOut record updated successfully.",
                Data = updatedStockOutDto
            };
        }

        // Delete StockOut (soft delete by setting DeletedAt)
        public async Task<ApiResponse<bool>> DeleteStockOutAsync(StockOutDeleteParams deleteParams)
        {
            var stockOut = await _context.StockOuts
                .FirstOrDefaultAsync(so => so.Id == deleteParams.Id && so.DeletedAt == null);

            if (stockOut == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "StockOut record not found.",
                    Data = false
                };
            }

            stockOut.DeletedAt = DateTime.Now; // Soft delete by setting the DeletedAt field
            _context.StockOuts.Update(stockOut);
            await _context.SaveChangesAsync();

            // Update the BranchProduct stock quantity back
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockOut.BranchId && bp.ProductId == stockOut.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                branchProduct.StockQuantity += stockOut.Quantity; // Restore the stock quantity
                _context.BranchProducts.Update(branchProduct);
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "StockOut record deleted successfully.",
                Data = true
            };
        }

        // Restore StockOut (undo soft delete)
        public async Task<ApiResponse<bool>> RestoreStockOutAsync(StockOutRestoreParams restoreParams)
        {
            var stockOut = await _context.StockOuts
                .FirstOrDefaultAsync(so => so.Id == restoreParams.Id && so.DeletedAt != null);

            if (stockOut == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "StockOut record not found.",
                    Data = false
                };
            }

            stockOut.DeletedAt = null; // Restore the record by setting DeletedAt to null
            _context.StockOuts.Update(stockOut);
            await _context.SaveChangesAsync();

            // Update BranchProduct stock quantity
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockOut.BranchId && bp.ProductId == stockOut.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                branchProduct.StockQuantity -= stockOut.Quantity; // Subtract the stock quantity from the restored record
                _context.BranchProducts.Update(branchProduct);
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "StockOut record restored successfully.",
                Data = true
            };
        }

        // Get StockOuts by Reference Number
        public async Task<ApiResponse<IEnumerable<StockOutDTO>>> GetStockOutByReferenceNumberAsync(string referenceNumber)
        {
            // Query the StockOuts table by the ReferenceNumber and ensure they are not deleted
            var stockOuts = await _context.StockOuts
                .Where(so => so.ReferenceNumber == referenceNumber && so.DeletedAt == null)
                .ToListAsync();

            if (!stockOuts.Any())
            {
                return new ApiResponse<IEnumerable<StockOutDTO>>
                {
                    Success = false,
                    Message = "No StockOut records found with the provided reference number.",
                    Data = null
                };
            }

            // Map the StockOut entities to StockOutDTOs
            var stockOutDtos = _mapper.Map<IEnumerable<StockOutDTO>>(stockOuts);

            return new ApiResponse<IEnumerable<StockOutDTO>>
            {
                Success = true,
                Message = "StockOut records retrieved successfully.",
                Data = stockOutDtos
            };
        }

        // Implementation of GetAllDeletedStockOutAsync

        public async Task<ApiResponse<List<StockOut>>> GetAllDeletedStockOutAsync()
        {
            // Fetch all soft-deleted records (where DeletedAt is not null)
            var deletedStockOut = await _context.StockOuts
                .Where(si => si.DeletedAt != null)  // Filter records where DeletedAt is not null
                .ToListAsync();

            if (deletedStockOut == null || deletedStockOut.Count == 0)
            {
                return new ApiResponse<List<StockOut>>
                {
                    Success = false,
                    Message = "No deleted StockIn records found.",
                    Data = null
                };
            }

            return new ApiResponse<List<StockOut>>
            {
                Success = true,
                Message = "Deleted StockIn records retrieved successfully.",
                Data = deletedStockOut
            };
        }
    }
}
