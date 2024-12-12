using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Models.Inventory;
using static Pacifica.API.Helper.GlobalEnums;

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
        public async Task<ApiResponse<IEnumerable<ResponseInventoryDTO>>> GetInventoriesAsync(int branchId, DateTime startDate, DateTime endDate)
        {
            var inventories = await _context.Inventories
                .Where(i => i.BranchId == branchId && i.InventoryDate >= startDate && i.InventoryDate <= endDate)
                .ToListAsync();

            if (!inventories.Any())
            {
                return new ApiResponse<IEnumerable<ResponseInventoryDTO>>
                {
                    Success = false,
                    Message = "No inventories found for the specified date range.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<ResponseInventoryDTO>>
            {
                Success = true,
                Message = "Weekly inventories retrieved successfully.",
                Data = _mapper.Map<IEnumerable<ResponseInventoryDTO>>(inventories)
            };
        }

        // Create a new weekly inventory
        public async Task<ApiResponse<ResponseInventoryDTO>> CreateInventoryAsync(CreateInventoryDTO inventoryDto)
        {
            var inventory = _mapper.Map<Inventory>(inventoryDto);

            // Automatically set the Year based on the InventoryDate
            inventory.Year = inventory.InventoryDate.Year;
            inventory.Month = inventory.InventoryDate.Month;
            inventory.Week = inventory.WeekNumber;

            // Explicitly set the IsComplete to 1 (or true)
            // inventory.IsComplete = 1;

            // Check if a Weekly Inventory with the same ProductId, BranchId, Month, and WeekNumber already exists
            var existingInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == inventory.ProductId
                                          && i.BranchId == inventory.BranchId
                                          && i.Month == inventory.Month
                                          && i.Week == inventory.Week);



            if (existingInventory != null)
            {
                return new ApiResponse<ResponseInventoryDTO>
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
                // Explicitly set the IsComplete to 1 (or true)
                inventory.IsCompleted = true;
            }

            inventory.CalculateDiscrepancy();  // This will calculate the discrepancy using SystemQuantity - ActualQuantity
            inventory.SumDiscrepancyValue = Math.Abs(inventory.Discrepancy) * inventory.CostPrice;

            // Add the new inventory to the database

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return new ApiResponse<ResponseInventoryDTO>
            {
                Success = true,
                Message = "Weekly inventory created successfully.",
                Data = _mapper.Map<ResponseInventoryDTO>(inventory)
            };
        }

        // Get a specific weekly inventory by ID
        public async Task<ApiResponse<ResponseInventoryDTO>> GetInventoryByIdAsync(int id)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return new ApiResponse<ResponseInventoryDTO>
                {
                    Success = false,
                    Message = "Inventory not found.",
                    Data = null
                };
            }

            return new ApiResponse<ResponseInventoryDTO>
            {
                Success = true,
                Message = "Inventory retrieved successfully.",
                Data = _mapper.Map<ResponseInventoryDTO>(inventory)
            };
        }

        // Update an existing weekly inventory
        public async Task<ApiResponse<ResponseInventoryDTO>> UpdateInventoryAsync(int id, UpdateInventoryDTO inventoryDto)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return new ApiResponse<ResponseInventoryDTO>
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

            return new ApiResponse<ResponseInventoryDTO>
            {
                Success = true,
                Message = "Weekly inventory updated successfully.",
                Data = _mapper.Map<ResponseInventoryDTO>(inventory)
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

        public async Task<ApiResponse<IEnumerable<ResponseInventoryDTO>>> GetFilteredInventoriesAsync(FilterInventoryParams filterParams)
        {
            try
            {
                // Query the WeeklyInventories table with the provided filters
                var filteredInventories = await _context.WeeklyInventories
                    .Where(i => i.BranchId == filterParams.BranchId
                                && (!filterParams.ProductId.HasValue || i.ProductId == filterParams.ProductId)
                                && i.Month == filterParams.Month
                                && i.Week == filterParams.Week
                                && i.IsCompleted != true) // Exclude records with isCompleted = 1

                    .ToListAsync();

                // If no records are found, return a message indicating no matching data
                if (filteredInventories == null || !filteredInventories.Any())
                {
                    return new ApiResponse<IEnumerable<ResponseInventoryDTO>>
                    {
                        Success = false,
                        Message = "No matching weekly inventories found.",
                        Data = null
                    };
                }

                // Map the filtered list to the DTOs and return it
                var response = _mapper.Map<IEnumerable<ResponseInventoryDTO>>(filteredInventories);
                return new ApiResponse<IEnumerable<ResponseInventoryDTO>>
                {
                    Success = true,
                    Message = "Filtered weekly inventories retrieved successfully.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during the database operation
                return new ApiResponse<IEnumerable<ResponseInventoryDTO>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the data: {ex.Message}",
                    Data = null
                };
            }
        }

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

        public async Task<ApiResponse<IEnumerable<WI_ResponseSearchBranchProduct>>> GetFilteredBranchProductAsync(WI_BranchProductSearchParams filterParams)
        {
            try
            {
                // Fetch WeeklyInventories that match the filters
                var weeklyInventoryMatches = await _context.Inventories
                    .Where(wi =>
                        wi.BranchId == filterParams.BranchId &&
                        wi.Week == filterParams.Week &&
                        wi.Month == filterParams.Month &&
                        wi.Year == filterParams.Year)
                    .Select(wi => new { wi.BranchId, wi.ProductId })
                    .ToListAsync();

                // Build the main query for BranchProducts
                var branchProductsQuery = _context.BranchProducts
                    .Where(bp => bp.BranchId == filterParams.BranchId)
                    .Join(_context.Products,
                        bp => bp.ProductId,
                        p => p.Id,
                        (bp, p) => new { BranchProduct = bp, Product = p })
                    .Join(_context.Categories,
                        wpp => wpp.Product.CategoryId,
                        c => c.Id,
                        (wpp, c) => new { wpp.BranchProduct, wpp.Product, Category = c })
                    .Join(_context.Suppliers,
                        wppc => wppc.Product.SupplierId,
                        s => s.Id,
                        (wppc, s) => new { wppc.BranchProduct, wppc.Product, wppc.Category, Supplier = s })
                    // Apply filters
                    .Where(bp =>
                        (filterParams.CategoryId == null || bp.Product.CategoryId == filterParams.CategoryId) &&
                        (filterParams.SupplierId == null || bp.Product.SupplierId == filterParams.SupplierId) &&
                        (string.IsNullOrEmpty(filterParams.SKU) || bp.Product.SKU.Contains(filterParams.SKU)))
                    .ToList() // Bring data into memory for client-side filtering
                    .Where(bp =>
                        !weeklyInventoryMatches.Any(wi =>
                            wi.BranchId == bp.BranchProduct.BranchId &&
                            wi.ProductId == bp.BranchProduct.ProductId)) // Client-side filtering for WeeklyInventory
                    .Select(bp => new WI_ResponseSearchBranchProduct
                    {
                        BranchId = bp.BranchProduct.BranchId,
                        ProductId = bp.BranchProduct.ProductId,
                        ProductName = bp.Product.ProductName,
                        SKU = bp.Product.SKU,
                        CategoryName = bp.Category.CategoryName,
                        SupplierName = bp.Supplier.SupplierName,
                        CategoryId = bp.Product.CategoryId,
                        SupplierId = bp.Product.SupplierId,
                        StockQuantity = bp.BranchProduct.StockQuantity
                    });

                // Convert to list
                var filteredProducts = branchProductsQuery.ToList();

                // Handle empty result
                if (!filteredProducts.Any())
                {
                    return new ApiResponse<IEnumerable<WI_ResponseSearchBranchProduct>>
                    {
                        Success = false,
                        Message = "No matching products found.",
                        Data = null
                    };
                }

                // Return successful response
                return new ApiResponse<IEnumerable<WI_ResponseSearchBranchProduct>>
                {
                    Success = true,
                    Message = "Branch products retrieved successfully.",
                    Data = filteredProducts
                };
            }
            catch (Exception ex)
            {
                // Handle errors
                return new ApiResponse<IEnumerable<WI_ResponseSearchBranchProduct>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the data: {ex.Message}",
                    Data = null
                };
            }
        }

   
   
    }
}
