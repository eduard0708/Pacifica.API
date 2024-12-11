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

            // Explicitly set the IsComplete to 1 (or true)
            // inventory.IsComplete = 1;

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
                // Explicitly set the IsComplete to 1 (or true)
                inventory.IsCompleted = true;
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
                                && i.Week == filterParams.Week
                                && i.IsCompleted != true) // Exclude records with isCompleted = 1

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

        //     public async Task<ApiResponse<IEnumerable<ResponseViewInventoryDTO>>> GetFilteredBranchProductAsync(WI_BranchProductSearchParams filterParams)
        //     {
        //         try
        //         {
        //             // // Join WeeklyInventories with BranchProduct, Products, Category, and Supplier
        //             // var filteredInventories = await _context.WeeklyInventories
        //             //     .Join(_context.BranchProducts,  // Join with BranchProduct to get the correct Product
        //             //         wi => new { wi.BranchId, wi.ProductId },
        //             //         bp => new { bp.BranchId, bp.ProductId },
        //             //         (wi, bp) => new { WeeklyInventory = wi, BranchProduct = bp })
        //             //     .Join(_context.Products,  // Join with Products to get Product data
        //             //         wp => wp.BranchProduct.ProductId,
        //             //         p => p.Id,
        //             //         (wp, p) => new { wp.WeeklyInventory, wp.BranchProduct, Product = p })
        //             //     .Join(_context.Categories,  // Join with Categories to get Category data
        //             //         wpp => wpp.Product.CategoryId,
        //             //         c => c.Id,
        //             //         (wpp, c) => new { wpp.WeeklyInventory, wpp.Product, Category = c, wpp.BranchProduct })
        //             //     .Join(_context.Suppliers,  // Join with Suppliers to get Supplier data
        //             //         wppc => wppc.Product.SupplierId,
        //             //         s => s.Id,
        //             //         (wppc, s) => new { wppc.WeeklyInventory, wppc.Product, wppc.Category, Supplier = s, wppc.BranchProduct })
        //             //     .Where(wppcs => wppcs.WeeklyInventory.BranchId == filterParams.BranchId
        //             //                     && (!filterParams.CategoryId.HasValue || wppcs.Product.CategoryId == filterParams.CategoryId.Value)
        //             //                     && (!filterParams.SupplierId.HasValue || wppcs.Product.SupplierId == filterParams.SupplierId.Value)
        //             //                     && (string.IsNullOrEmpty(filterParams.SKU) || wppcs.Product.SKU == filterParams.SKU)  // Check if SKU is null or empty
        //             //                     && wppcs.WeeklyInventory.Week == filterParams.Week
        //             //                     && wppcs.WeeklyInventory.Month == filterParams.Month
        //             //                     && wppcs.WeeklyInventory.Year == filterParams.Year
        //             //                     && (wppcs.WeeklyInventory.IsCompleted == false || wppcs.WeeklyInventory.IsCompleted == null))
        //             //     .Select(wppcs => new ResponseViewInventoryDTO
        //             //     {
        //             //         Id = wppcs.WeeklyInventory.Id,
        //             //         BranchId = wppcs.WeeklyInventory.BranchId,
        //             //         ProductId = wppcs.WeeklyInventory.ProductId,
        //             //         ProductName = wppcs.Product.ProductName,
        //             //         CategoryId = wppcs.Product.CategoryId,
        //             //         CategoryName = wppcs.Category.CategoryName,
        //             //         SupplierId = wppcs.Product.SupplierId,
        //             //         SupplierName = wppcs.Supplier.SupplierName,
        //             //         SKU = wppcs.Product.SKU,
        //             //         InventoryDate = wppcs.WeeklyInventory.InventoryDate,
        //             //         Year = wppcs.WeeklyInventory.Year,
        //             //         Month = wppcs.WeeklyInventory.Month,
        //             //         Week = wppcs.WeeklyInventory.Week,
        //             //         ActualQuantity = wppcs.WeeklyInventory.ActualQuantity,
        //             //         CostPrice = wppcs.WeeklyInventory.CostPrice,
        //             //         SystemQuantity = wppcs.WeeklyInventory.SystemQuantity,
        //             //         Discrepancy = wppcs.WeeklyInventory.Discrepancy,
        //             //         WeekNumber = wppcs.WeeklyInventory.WeekNumber,
        //             //         SumDiscrepancyValue = wppcs.WeeklyInventory.SumDiscrepancyValue,
        //             //         Remarks = wppcs.WeeklyInventory.Remarks,
        //             //         IsDeleted = wppcs.WeeklyInventory.IsDeleted,
        //             //         CreatedAt = wppcs.WeeklyInventory.CreatedAt,
        //             //         CreatedBy = wppcs.WeeklyInventory.CreatedBy
        //             //     })
        //             //     .ToListAsync();


        //            // Join BranchProducts with Products, Categories, Suppliers and WeeklyInventories
        // var filteredInventories = await _context.BranchProducts
        //     .Join(_context.Products, // Join BranchProduct with Products to get product data
        //         bp => bp.ProductId,
        //         p => p.Id,
        //         (bp, p) => new { BranchProduct = bp, Product = p })
        //     .Join(_context.Categories, // Join Products with Categories to get category data
        //         wpp => wpp.Product.CategoryId,
        //         c => c.Id,
        //         (wpp, c) => new { wpp.BranchProduct, wpp.Product, Category = c })
        //     .Join(_context.Suppliers, // Join Products with Suppliers to get supplier data
        //         wppc => wppc.Product.SupplierId,
        //         s => s.Id,
        //         (wppc, s) => new { wppc.BranchProduct, wppc.Product, wppc.Category, Supplier = s })
        //     .GroupJoin(_context.WeeklyInventories, // Left join with WeeklyInventories to include all products
        //         wppcs => new { wppcs.BranchProduct.BranchId, wppcs.BranchProduct.ProductId },
        //         wi => new { wi.BranchId, wi.ProductId },
        //         (wppcs, weeklyInventories) => new { wppcs.BranchProduct, wppcs.Product, wppcs.Category, wppcs.Supplier, WeeklyInventories = weeklyInventories })
        //     .Where(wppcs => !wppcs.WeeklyInventories.Any(wi =>
        //         wi.Week == filterParams.Week &&
        //         wi.Month == filterParams.Month &&
        //         wi.Year == filterParams.Year &&
        //         wi.IsCompleted == true) // Exclude if there is an entry in WeeklyInventories matching Week, Month, Year and IsCompleted is true
        //     )
        //     .Select(wppcs => new ResponseViewInventoryDTO
        //     {
        //         ProductId = wppcs.BranchProduct.ProductId,
        //         BranchId = wppcs.BranchProduct.BranchId,
        //         ProductName = wppcs.Product.ProductName,
        //         SKU = wppcs.Product.SKU,
        //         CategoryName = wppcs.Category.CategoryName,
        //         SupplierName = wppcs.Supplier.SupplierName,
        //         CategoryId = wppcs.Product.CategoryId,
        //         SupplierId = wppcs.Product.SupplierId,

        //         // Additional Fields:
        //         // These fields are based on the WeeklyInventory table or other related data
        //         IsDeleted = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().IsDeleted : false,
        //         InventoryDate = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().InventoryDate : null,
        //         WeekNumber = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().WeekNumber : null,
        //         ActualQuantity = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().ActualQuantity : 0,
        //         CostPrice = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().CostPrice : 0,
        //         SystemQuantity = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().SystemQuantity : 0,
        //         Discrepancy = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().Discrepancy : 0,
        //         SumDiscrepancyValue = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().SumDiscrepancyValue : 0,
        //         Remarks = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().Remarks : "",
        //         CreatedAt = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().CreatedAt : (DateTime?)null,
        //         CreatedBy = wppcs.WeeklyInventories.Any() ? wppcs.WeeklyInventories.First().CreatedBy : ""
        //     })
        //     .ToListAsync();



        //             // If no records are found, return a message indicating no matching data
        //             if (filteredInventories == null || !filteredInventories.Any())
        //             {
        //                 return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
        //                 {
        //                     Success = false,
        //                     Message = "No matching inventories found.",
        //                     Data = null
        //                 };
        //             }

        //             // Return the filtered inventories as DTOs
        //             return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
        //             {
        //                 Success = true,
        //                 Message = "Filtered inventories retrieved successfully.",
        //                 Data = filteredInventories
        //             };
        //         }
        //         catch (Exception ex)
        //         {
        //             // Handle any errors that might occur during the database operation
        //             return new ApiResponse<IEnumerable<ResponseViewInventoryDTO>>
        //             {
        //                 Success = false,
        //                 Message = $"An error occurred while retrieving the data: {ex.Message}",
        //                 Data = null
        //             };
        //         }
        //     }

        public async Task<ApiResponse<IEnumerable<Wi_ResponseSearchBranchProduct>>> GetFilteredBranchProductAsync(WI_BranchProductSearchParams filterParams)
        {
            try
            {
                // Start building the query for BranchProducts matching the BranchId
                var branchProductsQuery = _context.BranchProducts
                    .Where(bp => bp.BranchId == filterParams.BranchId) // Always filter by BranchId
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
                    .Select(wppcs => new
                    {
                        wppcs.BranchProduct.BranchId,
                        wppcs.BranchProduct.ProductId,
                        wppcs.Product.ProductName,
                        wppcs.Product.SKU,
                        wppcs.Category.CategoryName,
                        wppcs.Supplier.SupplierName,
                        wppcs.Product.CategoryId,
                        wppcs.Product.SupplierId
                    });

                // Apply optional filters based on the parameters provided
                if (!string.IsNullOrEmpty(filterParams.SKU))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.SKU.Contains(filterParams.SKU));
                }

                if (filterParams.SupplierId.HasValue)
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.SupplierId == filterParams.SupplierId);
                }

                if (filterParams.CategoryId.HasValue)
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.CategoryId == filterParams.CategoryId);
                }

                // Execute the query to get the list of branch products with the optional filters applied
                var branchProducts = await branchProductsQuery
                    .Select(wppcs => new Wi_ResponseSearchBranchProduct
                    {
                        BranchId = wppcs.BranchId,
                        ProductId = wppcs.ProductId,
                        ProductName = wppcs.ProductName,
                        SKU = wppcs.SKU,
                        CategoryName = wppcs.CategoryName,
                        SupplierName = wppcs.SupplierName,
                        CategoryId = wppcs.CategoryId,
                        SupplierId = wppcs.SupplierId,
                        SystemQuantity = 0 // Default value since no WeeklyInventory filtering is applied
                    })
                    .ToListAsync();

                // If no records are found, return a message indicating no matching data
                if (branchProducts == null || !branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<Wi_ResponseSearchBranchProduct>>
                    {
                        Success = false,
                        Message = "No matching products found.",
                        Data = null
                    };
                }

                // Return the branch products as DTOs
                return new ApiResponse<IEnumerable<Wi_ResponseSearchBranchProduct>>
                {
                    Success = true,
                    Message = "Branch products retrieved successfully.",
                    Data = branchProducts
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during the database operation
                return new ApiResponse<IEnumerable<Wi_ResponseSearchBranchProduct>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the data: {ex.Message}",
                    Data = null
                };
            }
        }



    }
}
