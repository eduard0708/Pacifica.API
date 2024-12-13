using Microsoft.EntityFrameworkCore;
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

        public async Task<ApiResponse<IEnumerable<ResponseNormalizeProduct>>> GetFilteredBranchProductWithDiscrepancyAsync(InventoryNormalizeParams filterParams)
        {
            try
            {
                // Fetch WeeklyInventories that match the filters
                var weeklyInventoryMatches = await _context.Inventories
                    .Where(wi =>
                        wi.BranchId == filterParams.BranchId &&
                        wi.Month == filterParams.Month &&
                        wi.Year == filterParams.Year)
                    .Select(wi => new { wi.BranchId, wi.ProductId, wi.Discrepancy, wi.DiscrepancyValue })
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
                        weeklyInventoryMatches.Any(wi =>
                            wi.BranchId == bp.BranchProduct.BranchId &&
                            wi.ProductId == bp.BranchProduct.ProductId &&
                            wi.Discrepancy != 0)) // Client-side filtering for WeeklyInventory and non-zero discrepancy
                    .Select(bp => new ResponseNormalizeProduct
                    {
                        BranchId = bp.BranchProduct.BranchId,
                        ProductId = bp.BranchProduct.ProductId,
                        ProductName = bp.Product.ProductName,
                        SKU = bp.Product.SKU,
                        CategoryName = bp.Category.CategoryName,
                        SupplierName = bp.Supplier.SupplierName,
                        CategoryId = bp.Product.CategoryId,
                        SupplierId = bp.Product.SupplierId,
                        // Discrepancy comes from the weeklyInventoryMatches
                        Discrepancy = weeklyInventoryMatches.FirstOrDefault(wi =>
                            wi.BranchId == bp.BranchProduct.BranchId &&
                            wi.ProductId == bp.BranchProduct.ProductId)?.Discrepancy ?? 0,
                        // Assign SumDiscrepancyValue directly from the Inventory table
                        SumDiscrepancyValue = weeklyInventoryMatches
                            .FirstOrDefault(wi =>
                                wi.BranchId == bp.BranchProduct.BranchId &&
                                wi.ProductId == bp.BranchProduct.ProductId)?.DiscrepancyValue ?? 0
                    });

                // Convert to list
                var filteredProducts = branchProductsQuery.ToList();

                // Handle empty result
                if (!filteredProducts.Any())
                {
                    return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
                    {
                        Success = false,
                        Message = "No matching products found with non-zero discrepancies.",
                        Data = null
                    };
                }

                // Return successful response
                return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
                {
                    Success = true,
                    Message = "Branch products retrieved successfully.",
                    Data = filteredProducts
                };
            }
            catch (Exception ex)
            {
                // Handle errors
                return new ApiResponse<IEnumerable<ResponseNormalizeProduct>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the data: {ex.Message}",
                    Data = null
                };
            }
        }



    }
}




