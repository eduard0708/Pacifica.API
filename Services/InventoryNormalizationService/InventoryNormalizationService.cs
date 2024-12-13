using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Models.Inventory;


namespace Pacifica.API.Services.InventoryNormalizationService
{
    public class InventoryNormalizationService : IInventoryNormalizationService
    {
        private readonly ApplicationDbContext _context;

        public InventoryNormalizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryNormalization>> GetAllAsync()
        {
            return await _context.InventoryNormalizations
                                 .Include(n => n.Inventory)
                                 .ToListAsync();
        }

        public async Task<InventoryNormalization?> GetByIdAsync(int id)
        {
            return await _context.InventoryNormalizations
                                 .Include(n => n.Inventory)
                                 .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<InventoryNormalization> CreateAsync(CreateInventoryNormalizationDto dto)
        {
            var normalization = new InventoryNormalization
            {
                InventoryId = dto.InventoryId,
                AdjustedQuantity = dto.ActualQuantity - dto.SystemQuantity, // Difference between Actual and System
                NormalizationDate = dto.NormalizationDate,
                SystemQuantity = dto.SystemQuantity,
                ActualQuantity = dto.ActualQuantity,
                CostPrice = dto.CostPrice,
                DiscrepancyValue = (dto.ActualQuantity - dto.SystemQuantity) * dto.CostPrice // AdjustedQuantity * CostPrice
            };

            _context.InventoryNormalizations.Add(normalization);
            await _context.SaveChangesAsync();

            return normalization;
        }

        public async Task<InventoryNormalization?> UpdateAsync(int id, InventoryNormalizationDto dto)
        {
            var normalization = await _context.InventoryNormalizations.FindAsync(id);
            if (normalization == null)
                return null;

            normalization.AdjustedQuantity = dto.AdjustedQuantity;
            normalization.NormalizationDate = dto.NormalizationDate;
            normalization.InventoryId = dto.InventoryId;

            _context.InventoryNormalizations.Update(normalization);
            await _context.SaveChangesAsync();

            return normalization;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var normalization = await _context.InventoryNormalizations.FindAsync(id);
            if (normalization == null)
                return false;

            _context.InventoryNormalizations.Remove(normalization);
            await _context.SaveChangesAsync();

            return true;
        }

        //  public async Task<ApiResponse<IEnumerable<ResponseNormalizeProduct>>> GetFilteredBranchProductWithDiscrepancyAsync(InventoryNormalizeParams filterParams)
        // // {
        // //     try
        // //     { 
        // //       var inventoryNormalizationData = await _context.InventoryNormalizations
        // //             .Where(ind => ind.Inventory != null &&
        // //                         ind.Inventory.BranchId == filterParams.BranchId &&
        // //                         ind.Inventory.Month == filterParams.Month &&
        // //                         ind.Inventory.Year == filterParams.Year)
        // //             .Include(ind => ind.Inventory) // Include Inventory
        // //             .ThenInclude(inventory => inventory!.BranchProduct) // Include BranchProduct details from Inventory
        // //             .ThenInclude(branchProduct => branchProduct!.Product) // Include Product details from BranchProduct
        // //             .ThenInclude(product => product!.Category) // Include Category from Product
        // //             .Include(ind => ind.Inventory!.BranchProduct!.Product!.Supplier) // Include Supplier directly from Product
        // //             .Where(ind => ind.Inventory!.BranchProduct != null &&
        // //                         (filterParams.CategoryId == null || ind.Inventory.BranchProduct.Product!.CategoryId == filterParams.CategoryId) &&
        // //                         (filterParams.SupplierId == null || ind.Inventory.BranchProduct.Product!.SupplierId == filterParams.SupplierId) &&
        // //                         (filterParams.SKU == null || ind.Inventory.BranchProduct.Product!.SKU == filterParams.SKU)) // Filtering by Category, Supplier, or SKU
        // //             .Select(ind => new ResponseNormalizeProduct
        // //             {
        // //                 BranchId = ind.Inventory!.BranchId,
        // //                 ProductId = ind.Inventory.BranchProduct!.ProductId,
        // //                 ProductName = ind.Inventory.BranchProduct.Product!.ProductName,
        // //                 SKU = ind.Inventory.BranchProduct.Product.SKU,
        // //                 CategoryId = ind.Inventory.BranchProduct.Product!.CategoryId,
        // //                 CategoryName = ind.Inventory.BranchProduct.Product.Category!.CategoryName, // Category Name from Product
        // //                 SupplierId = ind.Inventory.BranchProduct.Product.SupplierId,
        // //                 SupplierName = ind.Inventory.BranchProduct.Product.Supplier!.SupplierName, // Supplier Name from Product
        // //                 Discrepancy = ind.Inventory.Discrepancy,  // Assuming Discrepancy is in Inventory
        // //                 DiscrepancyValue = ind.Inventory.DiscrepancyValue // Assuming DiscrepancyValue is in Inventory
        // //             })
        // //             .ToListAsync();


        // //         // Handle empty result
        // //         if (!inventoryNormalizationData.Any())
        // //         {
        // //             return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
        // //             {
        // //                 Success = false,
        // //                 Message = "No matching products found with non-zero discrepancies.",
        // //                 Data = null
        // //             };
        // //         }

        // //         // Return successful response
        // //         return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
        // //         {
        // //             Success = true,
        // //             Message = "Branch products retrieved successfully.",
        // //             Data = inventoryNormalizationData
        // //         };
        // //     }
        // //     catch (Exception ex)
        // //     {
        // //         // Handle errors
        // //         return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
        // //         {
        // //             Success = false,
        // //             Message = $"An error occurred while retrieving the data: {ex.Message}",
        // //             Data = null
        // //         };
        // //     }
        // // }

        // public async Task<ApiResponse<ResponseNormalizeProduct>> GetFilteredBranchProductWithDiscrepancyAsync(InventoryNormalizeParams filterParams)
        // {
        //     try
        //     {
        //         var inventoryNormalizationData = await _context.InventoryNormalizations
        //             .Where(ind => ind.Inventory != null &&
        //                         ind.Inventory.BranchId == filterParams.BranchId &&
        //                         ind.Inventory.Month == filterParams.Month &&
        //                         ind.Inventory.Year == filterParams.Year)
        //             .Include(ind => ind.Inventory) // Include Inventory
        //                 .ThenInclude(inventory => inventory!.BranchProduct) // Include BranchProduct details from Inventory
        //                 .ThenInclude(branchProduct => branchProduct!.Product) // Include Product details from BranchProduct
        //                 .ThenInclude(product => product!.Category) // Include Category from Product
        //             .Include(ind => ind.Inventory!.BranchProduct!.Product!.Supplier) // Include Supplier directly from Product
        //             .Where(ind => ind.Inventory!.BranchProduct != null &&
        //                         (filterParams.CategoryId == null || ind.Inventory.BranchProduct.Product!.CategoryId == filterParams.CategoryId) &&
        //                         (filterParams.SupplierId == null || ind.Inventory.BranchProduct.Product!.SupplierId == filterParams.SupplierId) &&
        //                         (filterParams.SKU == null || ind.Inventory.BranchProduct.Product!.SKU == filterParams.SKU)) // Filtering by Category, Supplier, or SKU
        //             .Select(ind => new ResponseNormalizeProduct
        //             {
        //                 BranchId = ind.Inventory!.BranchId,
        //                 ProductId = ind.Inventory.BranchProduct!.ProductId,
        //                 ProductName = ind.Inventory.BranchProduct.Product!.ProductName,
        //                 SKU = ind.Inventory.BranchProduct.Product.SKU,
        //                 CategoryId = ind.Inventory.BranchProduct.Product!.CategoryId,
        //                 CategoryName = ind.Inventory.BranchProduct.Product.Category!.CategoryName, // Category Name from Product
        //                 SupplierId = ind.Inventory.BranchProduct.Product.SupplierId,
        //                 SupplierName = ind.Inventory.BranchProduct.Product.Supplier!.SupplierName, // Supplier Name from Product
        //                 Discrepancy = ind.Inventory.Discrepancy,  // Assuming Discrepancy is in Inventory
        //                 DiscrepancyValue = ind.Inventory.DiscrepancyValue // Assuming DiscrepancyValue is in Inventory
        //             })
        //             .FirstOrDefaultAsync(); // Get the first matching result or null

        //         if (inventoryNormalizationData == null)
        //         {
        //             return new ApiResponse<ResponseNormalizeProduct> { Success = false, Message = "No product found for the provided SKU." };
        //         }

        //         return new ApiResponse<ResponseNormalizeProduct> { Success = true, Data = inventoryNormalizationData };
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception and return a meaningful error response
        //         return new ApiResponse<ResponseNormalizeProduct> { Success = false, Message = "An error occurred while retrieving the data." };
        //     }
        // }
        public async Task<ApiResponse<IEnumerable<ResponseNormalizeProduct>>> GetFilteredBranchProductWithDiscrepancyAsync(InventoryNormalizeParams filterParams)
        {
            try
            {
                var query = _context.InventoryNormalizations
                    .Where(ind => ind.Inventory != null &&
                                ind.Inventory.BranchId == filterParams.BranchId &&
                                ind.Inventory.Month == filterParams.Month &&
                                ind.Inventory.Year == filterParams.Year)
                    .Include(ind => ind.Inventory) // Include Inventory
                        .ThenInclude(inventory => inventory!.BranchProduct) // Include BranchProduct details from Inventory
                        .ThenInclude(branchProduct => branchProduct!.Product) // Include Product details from BranchProduct
                        .ThenInclude(product => product!.Category) // Include Category from Product
                    .Include(ind => ind.Inventory!.BranchProduct!.Product!.Supplier) // Include Supplier directly from Product
                    .Where(ind => ind.Inventory!.BranchProduct != null &&
                                (filterParams.CategoryId == null || ind.Inventory.BranchProduct.Product!.CategoryId == filterParams.CategoryId) &&
                                (filterParams.SupplierId == null || ind.Inventory.BranchProduct.Product!.SupplierId == filterParams.SupplierId));

                // Apply SKU filter conditionally
                if (!string.IsNullOrEmpty(filterParams.SKU))
                {
                    query = query.Where(ind => ind.Inventory!.BranchProduct!.Product!.SKU == filterParams.SKU);
                }

                var inventoryNormalizationData = await query
                    .Select(ind => new ResponseNormalizeProduct
                    {
                        BranchId = ind.Inventory!.BranchId,
                        ProductId = ind.Inventory.BranchProduct!.ProductId,
                        ProductName = ind.Inventory.BranchProduct.Product!.ProductName,
                        SKU = ind.Inventory.BranchProduct.Product.SKU,
                        CategoryId = ind.Inventory.BranchProduct.Product!.CategoryId,
                        CategoryName = ind.Inventory.BranchProduct.Product.Category!.CategoryName, // Category Name from Product
                        SupplierId = ind.Inventory.BranchProduct.Product.SupplierId,
                        SupplierName = ind.Inventory.BranchProduct.Product.Supplier!.SupplierName, // Supplier Name from Product
                        Discrepancy = ind.Inventory.Discrepancy,  // Assuming Discrepancy is in Inventory
                        DiscrepancyValue = ind.Inventory.DiscrepancyValue // Assuming DiscrepancyValue is in Inventory
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<ResponseNormalizeProduct>> { Success = true, Data = inventoryNormalizationData };
            }
            catch (Exception ex)
            {
                // Log the exception and return a meaningful error response
                return new ApiResponse<IEnumerable<ResponseNormalizeProduct>> { Success = false, Message = "An error occurred while retrieving the data." };
            }
        }


    }
}
