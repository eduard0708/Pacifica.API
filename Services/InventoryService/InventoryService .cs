using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Models.Inventory;
using Pacifica.API.Helper;
using System.Collections.Specialized;  // Assuming this is where ApiResponse is defined

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

        // Get a list of all weekly inventories
        public async Task<ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>> GetWeeklyInventoriesAsync(int branchId, DateTime startDate, DateTime endDate)
        {
            var inventories = await _context.WeeklyInventories
                .Where(i => i.BranchId == branchId && i.InventoryDate >= startDate && i.InventoryDate <= endDate)
                .ToListAsync();

            if (!inventories.Any())
            {
                return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
                {
                    Success = false,
                    Message = "No inventories found for the specified date range.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
            {
                Success = true,
                Message = "Weekly inventories retrieved successfully.",
                Data = _mapper.Map<IEnumerable<ResponseWeeklyInventoryDTO>>(inventories)
            };
        }

        // Create a new weekly inventory
        public async Task<ApiResponse<ResponseWeeklyInventoryDTO>> CreateWeeklyInventoryAsync(CreateWeeklyInventoryDTO inventoryDto)
        {
            var inventory = _mapper.Map<WeeklyInventory>(inventoryDto);

            // Automatically set the Year based on the InventoryDate
            inventory.Year = inventory.InventoryDate.Year;
            inventory.Month = inventory.InventoryDate.Month;
            inventory.Week = inventory.WeekNumber;

            // Check if a Weekly Inventory with the same ProductId, BranchId, Month, and WeekNumber already exists
            var existingInventory = await _context.WeeklyInventories
                .FirstOrDefaultAsync(i => i.ProductId == inventory.ProductId
                                          && i.BranchId == inventory.BranchId
                                          && i.Month == inventory.Month
                                          && i.Week == inventory.Week);

            if (existingInventory != null)
            {
                return new ApiResponse<ResponseWeeklyInventoryDTO>
                {
                    Success = false,
                    Message = "Weekly inventory for this product in the specified week and month already exists.",
                    Data = null
                };
            }

            // Get the system quantity and Cost from the BranchProducts table
            var branchProduct = await _context.BranchProducts
               .Where(bp => bp.ProductId == inventory.ProductId)
               .Select(bp => new { bp.StockQuantity, bp.CostPrice })
               .FirstOrDefaultAsync();


            if (branchProduct != null)
            {
                inventory.SystemQuantity = branchProduct.StockQuantity;
                inventory.CostPrice = branchProduct.CostPrice;
            }

            inventory.CalculateDiscrepancy();  // This will calculate the discrepancy using SystemQuantity - ActualQuantity
            inventory.SumDiscrepancyValue = Math.Abs(inventory.Discrepancy) * inventory.CostPrice;

            // Add the new inventory to the database
            _context.WeeklyInventories.Add(inventory);
            await _context.SaveChangesAsync();

            return new ApiResponse<ResponseWeeklyInventoryDTO>
            {
                Success = true,
                Message = "Weekly inventory created successfully.",
                Data = _mapper.Map<ResponseWeeklyInventoryDTO>(inventory)
            };
        }

        // Get a specific weekly inventory by ID
        public async Task<ApiResponse<ResponseWeeklyInventoryDTO>> GetWeeklyInventoryByIdAsync(int id)
        {
            var inventory = await _context.WeeklyInventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return new ApiResponse<ResponseWeeklyInventoryDTO>
                {
                    Success = false,
                    Message = "Inventory not found.",
                    Data = null
                };
            }

            return new ApiResponse<ResponseWeeklyInventoryDTO>
            {
                Success = true,
                Message = "Inventory retrieved successfully.",
                Data = _mapper.Map<ResponseWeeklyInventoryDTO>(inventory)
            };
        }

        // Update an existing weekly inventory
        public async Task<ApiResponse<ResponseWeeklyInventoryDTO>> UpdateWeeklyInventoryAsync(int id, UpdateWeeklyInventoryDTO inventoryDto)
        {
            var inventory = await _context.WeeklyInventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return new ApiResponse<ResponseWeeklyInventoryDTO>
                {
                    Success = false,
                    Message = "Inventory not found.",
                    Data = null
                };
            }

            // Update properties from DTO
            inventory.InventoryDate = inventoryDto.InventoryDate;
            inventory.ActualQuantity = inventoryDto.ActualQuantity;
            inventory.SystemQuantity = inventoryDto.SystemQuantity;
            // inventory.Month = inventoryDto.Month;
            // inventory.Year = inventoryDto.Year;

            // Automatically recalculate discrepancy
            inventory.CalculateDiscrepancy();  // Recalculate the discrepancy

            await _context.SaveChangesAsync();

            return new ApiResponse<ResponseWeeklyInventoryDTO>
            {
                Success = true,
                Message = "Weekly inventory updated successfully.",
                Data = _mapper.Map<ResponseWeeklyInventoryDTO>(inventory)
            };
        }

        // Calculate discrepancy (positive or negative)
        public Task<ApiResponse<int>> CalculateDiscrepancyAsync(int systemQuantity, int actualQuantity)
        {
            var discrepancy = systemQuantity - actualQuantity;

            return Task.FromResult(new ApiResponse<int>
            {
                Success = true,
                Message = "Discrepancy calculated successfully.",
                Data = discrepancy
            });
        }


        // Get Weekly Inventories filtered by BranchId, ProductId, Month, and WeekNumber
        // public async Task<ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>> GetFilteredWeeklyInventoriesAsync(FilterWeeklyInventoryParams filterParams)
        // {
        //     try
        //     {
        //         // Query the WeeklyInventories table with the provided filters
        //         var filteredInventories = await _context.WeeklyInventories
        //             .Where(i => i.BranchId == filterParams.BranchId
        //                     && i.ProductId == filterParams.ProductId
        //                     && i.Month == filterParams.Month
        //                     && i.Week == filterParams.Week)
        //             .ToListAsync();

        //         // If no records are found, return a message indicating no matching data
        //         if (filteredInventories == null || !filteredInventories.Any())
        //         {
        //             return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
        //             {
        //                 Success = false,
        //                 Message = "No matching weekly inventories found.",
        //                 Data = null
        //             };
        //         }

        //         // Map the filtered list to the DTOs and return it
        //         var response = _mapper.Map<IEnumerable<ResponseWeeklyInventoryDTO>>(filteredInventories);
        //         return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
        //         {
        //             Success = true,
        //             Message = "Filtered weekly inventories retrieved successfully.",
        //             Data = response
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle any errors that might occur during the database operation
        //         return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
        //         {
        //             Success = false,
        //             Message = $"An error occurred while retrieving the data: {ex.Message}",
        //             Data = null
        //         };
        //     }
        // }

        public async Task<ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>> GetFilteredWeeklyInventoriesAsync(FilterWeeklyInventoryParams filterParams)
        {
            try
            {
                // Query the WeeklyInventories table with the provided filters
                var filteredInventories = await _context.WeeklyInventories
                    .Where(i => i.BranchId == filterParams.BranchId
                                && (!filterParams.ProductId.HasValue || i.ProductId == filterParams.ProductId)
                                && i.Month == filterParams.Month
                                && i.Week == filterParams.Week)
                    .ToListAsync();

                // If no records are found, return a message indicating no matching data
                if (filteredInventories == null || !filteredInventories.Any())
                {
                    return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
                    {
                        Success = false,
                        Message = "No matching weekly inventories found.",
                        Data = null
                    };
                }

                // Map the filtered list to the DTOs and return it
                var response = _mapper.Map<IEnumerable<ResponseWeeklyInventoryDTO>>(filteredInventories);
                return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
                {
                    Success = true,
                    Message = "Filtered weekly inventories retrieved successfully.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during the database operation
                return new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the data: {ex.Message}",
                    Data = null
                };
            }
        }


        // public async Task<ApiResponse<IEnumerable<Object>>> GetViewInventoriesAsync(ViewInventoryParams filterParams)
        // {
        //     try
        //     {
        //         // Join WeeklyInventories with Products to apply filters for CategoryId, SupplierId, and SKU
        //         var filteredInventories = await _context.WeeklyInventories
        //             .Join(_context.Products,
        //                 wi => wi.ProductId,
        //                 p => p.Id,
        //                 (wi, p) => new { WeeklyInventory = wi, Product = p })  // Join WeeklyInventories with Products
        //             .Where(wp => wp.WeeklyInventory.BranchId == filterParams.BranchId
        //                         && (!filterParams.CategoryId.HasValue || wp.Product.CategoryId == filterParams.CategoryId.Value)
        //                         && (!filterParams.SupplierId.HasValue || wp.Product.SupplierId == filterParams.SupplierId.Value)
        //                         && (string.IsNullOrEmpty(filterParams.SKU) || wp.Product.SKU == filterParams.SKU)
        //                         && wp.WeeklyInventory.Week == filterParams.Week
        //                         && wp.WeeklyInventory.Month == filterParams.Month
        //                         && wp.WeeklyInventory.Year == filterParams.Year)
        //             .Select(wp => wp.WeeklyInventory)  // Select WeeklyInventory for the final result
        //             .ToListAsync();

        //         // If no records are found, return a message indicating no matching data
        //         if (filteredInventories == null || !filteredInventories.Any())
        //         {
        //             return new ApiResponse<IEnumerable<Object>>
        //             {
        //                 Success = false,
        //                 Message = "No matching inventories found.",
        //                 Data = null
        //             };
        //         }

        //         // Map the filtered list to the DTOs and return it
        //         // var response = _mapper.Map<IEnumerable<ResponseViewInventoryDTO>>(filteredInventories);
        //         return new ApiResponse<IEnumerable<Object>>
        //         {
        //             Success = true,
        //             Message = "Filtered inventories retrieved successfully.",
        //             Data = filteredInventories
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle any errors that might occur during the database operation
        //         return new ApiResponse<IEnumerable<Object>>
        //         {
        //             Success = false,
        //             Message = $"An error occurred while retrieving the data: {ex.Message}",
        //             Data = null
        //         };
        //     }
        // }

public async Task<ApiResponse<IEnumerable<ResponseViewInventoryDTO>>> GetViewInventoriesAsync(ViewInventoryParams filterParams)
{
    try
    {
        // Join WeeklyInventories with BranchProduct and then with Products, Category, and Supplier
        var filteredInventories = await _context.WeeklyInventories
            .Join(_context.BranchProducts,  // Join with BranchProduct to get the correct Product
                wi => new { wi.BranchId, wi.ProductId },
                bp => new { bp.BranchId, bp.ProductId },
                (wi, bp) => new { WeeklyInventory = wi, BranchProduct = bp })
            .Join(_context.Products,  // Join with Products to get Product data
                wp => wp.BranchProduct.ProductId,
                p => p.Id,
                (wp, p) => new { wp.WeeklyInventory, wp.BranchProduct, Product = p })
            .Join(_context.Categories,  // Join with Categories to get Category data
                wpp => wpp.Product.CategoryId,
                c => c.Id,
                (wpp, c) => new { wpp.WeeklyInventory, wpp.Product, Category = c, wpp.BranchProduct })
            .Join(_context.Suppliers,  // Join with Suppliers to get Supplier data
                wppc => wppc.Product.SupplierId,
                s => s.Id,
                (wppc, s) => new { wppc.WeeklyInventory, wppc.Product, wppc.Category, Supplier = s, wppc.BranchProduct })
            .Where(wppcs => wppcs.WeeklyInventory.BranchId == filterParams.BranchId
                            && (!filterParams.CategoryId.HasValue || wppcs.Product.CategoryId == filterParams.CategoryId.Value)
                            && (!filterParams.SupplierId.HasValue || wppcs.Product.SupplierId == filterParams.SupplierId.Value)
                            && (string.IsNullOrEmpty(filterParams.SKU) || wppcs.Product.SKU == filterParams.SKU)
                            && wppcs.WeeklyInventory.Week == filterParams.Week
                            && wppcs.WeeklyInventory.Month == filterParams.Month
                            && wppcs.WeeklyInventory.Year == filterParams.Year)
            .Select(wppcs => new ResponseViewInventoryDTO
            {
                Id = wppcs.WeeklyInventory.Id,
                BranchId = wppcs.WeeklyInventory.BranchId,
                ProductId = wppcs.WeeklyInventory.ProductId,
                ProductName = wppcs.Product.ProductName,  // Access ProductName from the Product entity
                CategoryId = wppcs.Product.CategoryId,
                CategoryName = wppcs.Category.CategoryName, // Access CategoryName from the Category entity
                SupplierId = wppcs.Product.SupplierId,
                SupplierName = wppcs.Supplier.SupplierName, // Access SupplierName from the Supplier entity
                SKU = wppcs.Product.SKU, // Access SKU from the Product entity
                InventoryDate = wppcs.WeeklyInventory.InventoryDate,
                Year = wppcs.WeeklyInventory.Year,
                Month = wppcs.WeeklyInventory.Month,
                Week = wppcs.WeeklyInventory.Week,
                ActualQuantity = wppcs.WeeklyInventory.ActualQuantity,
                CostPrice = wppcs.WeeklyInventory.CostPrice,
                SystemQuantity = wppcs.WeeklyInventory.SystemQuantity,
                Discrepancy = wppcs.WeeklyInventory.Discrepancy,
                WeekNumber = wppcs.WeeklyInventory.WeekNumber,
                SumDiscrepancyValue = wppcs.WeeklyInventory.SumDiscrepancyValue,
                Remarks = wppcs.WeeklyInventory.Remarks,
                IsDeleted = wppcs.WeeklyInventory.IsDeleted,
                CreatedAt = wppcs.WeeklyInventory.CreatedAt,
                CreatedBy = wppcs.WeeklyInventory.CreatedBy
            })
            .ToListAsync();

        // If no records are found, return a message indicating no matching data
        if (filteredInventories == null || !filteredInventories.Any())
        {
            return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
            {
                Success = false,
                Message = "No matching inventories found.",
                Data = null
            };
        }

        // Return the filtered inventories as DTOs
        return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
        {
            Success = true,
            Message = "Filtered inventories retrieved successfully.",
            Data = filteredInventories
        };
    }
    catch (Exception ex)
    {
        // Handle any errors that might occur during the database operation
        return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
        {
            Success = false,
            Message = $"An error occurred while retrieving the data: {ex.Message}",
            Data = null
        };
    }
}



    }
}
