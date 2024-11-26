using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Models.Transaction;

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

            // Map the updated values to the existing entity
            _mapper.Map(stockInDto, existingStockIn);
            existingStockIn.UpdatedAt = DateTime.Now;  // Update timestamp
            existingStockIn.UpdatedBy = stockInDto.CreatedBy;  // Assuming createdBy is passed in DTO

            _context.StockIns.Update(existingStockIn);
            await _context.SaveChangesAsync();

            // Update the BranchProduct stock quantity
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == stockInDto.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                // If the StockIn is updated, adjust the quantity accordingly (new quantity - old quantity)
                branchProduct.StockQuantity += stockInDto.Quantity - existingStockIn.Quantity; // Adjust stock
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();
            }

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

        // Update existing StockIn by Reference Number
        public async Task<ApiResponse<StockInDTO>> UpdateStockInByReferenceNumberAsync(string referenceNumber, StockInUpdateDTO stockInDto)
        {
            var existingStockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.ReferenceNumber == referenceNumber && si.DeletedAt == null);

            if (existingStockIn == null)
            {
                return new ApiResponse<StockInDTO>
                {
                    Success = false,
                    Message = "StockIn record not found.",
                    Data = null
                };
            }

            // Map the updated values to the existing entity
            _mapper.Map(stockInDto, existingStockIn);
            existingStockIn.UpdatedAt = DateTime.Now;  // Update timestamp
            existingStockIn.UpdatedBy = stockInDto.CreatedBy;  // Assuming createdBy is passed in DTO

            _context.StockIns.Update(existingStockIn);
            await _context.SaveChangesAsync();

            var updatedStockInDto = _mapper.Map<StockInDTO>(existingStockIn);

            return new ApiResponse<StockInDTO>
            {
                Success = true,
                Message = "StockIn record updated successfully.",
                Data = updatedStockInDto
            };
        }

        // Delete StockIn (soft delete by setting DeletedAt)
        public async Task<ApiResponse<bool>> DeleteStockInAsync(int id)
        {
            var stockIn = await _context.StockIns
                .FirstOrDefaultAsync(si => si.Id == id && si.DeletedAt == null);

            if (stockIn == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "StockIn record not found.",
                    Data = false
                };
            }

            stockIn.DeletedAt = DateTime.Now;
            stockIn.DeletedBy = "System";  // Ideally this should come from the authenticated user

            _context.StockIns.Update(stockIn);
            await _context.SaveChangesAsync();

            // Optionally, you can update the BranchProduct stock quantity in case of deletion
            var branchProduct = await _context.BranchProducts
                .FirstOrDefaultAsync(bp => bp.BranchId == stockIn.BranchId && bp.ProductId == stockIn.ProductId && bp.DeletedAt == null);

            if (branchProduct != null)
            {
                branchProduct.StockQuantity -= stockIn.Quantity; // Remove the quantity from the stock
                _context.BranchProducts.Update(branchProduct);
                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "StockIn record deleted successfully.",
                Data = true
            };
        }

        // Update multiple StockIn records
        public async Task<ApiResponse<List<StockInDTO>>> UpdateStockInsAsync(List<StockInUpdateDTO> stockInDtos)
        {
            var updatedStockInDtos = new List<StockInDTO>();
            var failedUpdates = new List<string>();

            foreach (var stockInDto in stockInDtos)
            {
                var existingStockIn = await _context.StockIns
                    .FirstOrDefaultAsync(si => si.Id == stockInDto.Id && si.DeletedAt == null);

                if (existingStockIn == null)
                {
                    failedUpdates.Add($"StockIn record with ID {stockInDto.Id} not found.");
                    continue; // Skip this update and move on to the next
                }

                // Map the updated values to the existing entity
                _mapper.Map(stockInDto, existingStockIn);
                existingStockIn.UpdatedAt = DateTime.Now;  // Update timestamp
                existingStockIn.UpdatedBy = stockInDto.CreatedBy;  // Assuming createdBy is passed in DTO

                _context.StockIns.Update(existingStockIn);
                await _context.SaveChangesAsync();

                // Update the BranchProduct stock quantity
                var branchProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == stockInDto.BranchId && bp.ProductId == stockInDto.ProductId && bp.DeletedAt == null);

                if (branchProduct != null)
                {
                    // If the StockIn is updated, adjust the quantity accordingly (new quantity - old quantity)
                    branchProduct.StockQuantity += stockInDto.Quantity - existingStockIn.Quantity; // Adjust stock
                    _context.BranchProducts.Update(branchProduct);
                    await _context.SaveChangesAsync();
                }

                var updatedStockInDto = _mapper.Map<StockInDTO>(existingStockIn);
                updatedStockInDtos.Add(updatedStockInDto);
            }

            if (failedUpdates.Any())
            {
                return new ApiResponse<List<StockInDTO>>
                {
                    Success = false,
                    Message = $"Some updates failed: {string.Join(", ", failedUpdates)}",
                    Data = updatedStockInDtos
                };
            }

            return new ApiResponse<List<StockInDTO>>
            {
                Success = true,
                Message = "StockIn records updated successfully.",
                Data = updatedStockInDtos
            };
        }


    }
}
