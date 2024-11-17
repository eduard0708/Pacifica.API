using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Services.BranchProductService
{
    public class BranchProductService : IBranchProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BranchProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Existing method to get all products by branch
        public async Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId)
        {
            try
            {
                var branchProducts = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.DeletedAt == null)
                    .Include(bp => bp.Product)
                        .ThenInclude(p => p!.Category)
                    .ToListAsync();

                if (!branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
                    {
                        Success = false,
                        Message = "No products found for this branch.",
                        Data = null
                    };
                }

                var branch = await _context.Branches.FindAsync(branchId);

                var responseDtos = branchProducts.Select(bp => new GetAllBranchProductResponseDto
                {
                    BranchId = branch!.Id,
                    BranchName = branch.BranchName,
                    ProductId = bp.Product!.Id,
                    ProductStatusId = bp.StatusId,
                    ProductName = bp.Product.ProductName,
                    ProductCategory = bp.Product.Category!.CategoryName,
                    CostPrice = bp.CostPrice,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
                    Remarks = bp.Remarks,
                    SKU = bp.Product.SKU,
                    IsActive = bp.IsActive
                }).ToList();

                return new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = responseDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching products: {ex.Message}",
                    Data = null
                };
            }
        }

        // New method to filter products by the new DTO structure
        public async Task<ApiResponse<IEnumerable<GetBranchProductFilterDto>>> GetProductsFilteredByBranchAsync(int branchId, string? productCategory = null, string? sku = null, string? productName = null)
        {
            try
            {
                // Start with filtering the branch products by branchId and DeletedAt
                var branchProductsQuery = _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.DeletedAt == null);

                // Apply the filtering for productCategory if provided
                if (!string.IsNullOrEmpty(productCategory))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.Category!.CategoryName!.Contains(productCategory));
                }

                // Apply the filtering for SKU if provided
                if (!string.IsNullOrEmpty(sku))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.SKU.Contains(sku));
                }

                // Apply the filtering for SKU if provided
                if (!string.IsNullOrEmpty(productName))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.ProductName.Contains(productName));
                }

                // Now, include the related entities after applying filters

                branchProductsQuery = branchProductsQuery.Include(b => b.Branch)
                                                            .Include(ps => ps.Status)
                                                            .Include(bp => bp.Product)
                                                            .ThenInclude(p => p!.Category);

                // Fetch the filtered products
                var branchProducts = await branchProductsQuery.ToListAsync();

                if (!branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<GetBranchProductFilterDto>>
                    {
                        Success = false,
                        Message = "No products found matching the filter criteria.",
                        Data = null
                    };
                }

                var responseDtos = branchProducts.Select(bp => new GetBranchProductFilterDto
                {

                    BranchId = bp.Branch != null ? bp.Branch.Id : 0,
                    BranchName = bp.Branch != null ? bp.Branch.BranchName : "Unknown",

                    Product = new BranchProduct_ProductDto
                    {
                        ProductId = bp.Product != null ? bp.Product.Id : 0,
                        ProductName = bp.Product?.ProductName ?? "Unknown",
                        CategoryId = bp.Product?.Category?.Id ?? 0,
                        CategoryName = bp.Product?.Category?.CategoryName ?? "Unknown",
                        SupplierId = bp.Product?.Supplier?.Id ?? 0,
                        SupplierName = bp.Product?.Supplier?.SupplierName ?? "Unknown",
                        Remarks = bp.Remarks ?? string.Empty,
                        SKU = bp.Product?.SKU ?? "Unknown"
                    },

                    CostPrice = bp.CostPrice,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
                    IsActive = bp.IsActive

                }).ToList();

                return new ApiResponse<IEnumerable<GetBranchProductFilterDto>>
                {
                    Success = true,
                    Message = "Filtered products retrieved successfully.",
                    Data = responseDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GetBranchProductFilterDto>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching filtered products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts)
        {
            var responses = new List<BranchProductResponseDto>();
            var errors = new List<string>(); // Track errors for individual products

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var branchProduct in branchProducts)
                {
                    // Check if the product exists and has not been deleted
                    var productExists = await _context.Products
                        .AnyAsync(p => p.Id == branchProduct.ProductId && p.DeletedAt == null);

                    if (!productExists)
                    {
                        errors.Add($"Product ID {branchProduct.ProductId} does not exist or has been deleted.");
                        continue;
                    }

                    // Validate if the StatusId exists in the Statuses table
                    var statusExists = await _context.Statuses
                        .AnyAsync(s => s.Id == branchProduct.StatusId);

                    if (!statusExists)
                    {
                        errors.Add($"StatusId {branchProduct.StatusId} does not exist.");
                        continue;
                    }

                    // Check if the product is already associated with the branch
                    var existingProduct = await _context.BranchProducts
                        .FirstOrDefaultAsync(bp => bp.BranchId == branchProduct.BranchId
                                                   && bp.ProductId == branchProduct.ProductId
                                                   && bp.DeletedAt == null);

                    if (existingProduct != null)
                    {
                        errors.Add($"Product ID {branchProduct.ProductId} is already associated with Branch ID {branchProduct.BranchId}.");
                        continue;
                    }

                    // Add the product to the BranchProducts table
                    _context.BranchProducts.Add(branchProduct);
                }

                // If there were no errors, save all changes to the database
                if (!errors.Any())
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Map responses for all successfully added products
                    foreach (var branchProduct in branchProducts)
                    {
                        var branch = await _context.Branches.FindAsync(branchProduct.BranchId);
                        var product = await _context.Products
                            .Include(p => p.Category)
                            .FirstOrDefaultAsync(p => p.Id == branchProduct.ProductId);

                        if (branch != null && product != null)
                        {
                            var responseDto = new BranchProductResponseDto
                            {
                                BranchId = branch.Id,
                                BranchName = branch.BranchName,

                                ProductId = product.Id,
                                ProductName = product.ProductName,

                                ProductCategoryId = product.CategoryId,
                                ProductCategory = product.Category?.CategoryName ?? "No Category",

                                ProductSupplierId = product.SupplierId,
                                ProductSupplier = product.Supplier?.SupplierName ?? "No Supplier",

                                ProductSKU = product.SKU,

                                StatusId = branchProduct.StatusId,
                                Status = branchProduct.Status?.StatusName ?? "No Status",

                                CostPrice = branchProduct.CostPrice,
                                RetailPrice = branchProduct.RetailPrice,
                                StockQuantity = branchProduct.StockQuantity,

                                IsActive = branchProduct.IsActive,
                                CreatedBy = branchProduct.CreatedBy
                            };

                            responses.Add(responseDto);
                        }
                        else
                        {
                            errors.Add($"Branch or Product not found for BranchId: {branchProduct.BranchId} and ProductId: {branchProduct.ProductId}.");
                        }
                    }

                    return new ApiResponse<IEnumerable<BranchProductResponseDto>>
                    {
                        Success = true,
                        Message = "Products added successfully.",
                        Data = responses
                    };
                }
                else
                {
                    // Rollback transaction if errors
                    await transaction.RollbackAsync();

                    return new ApiResponse<IEnumerable<BranchProductResponseDto>>
                    {
                        Success = false,
                        Message = $"Some products could not be added: {string.Join(", ", errors)}",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an unexpected error
                await transaction.RollbackAsync();

                return new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = $"Error occurred while adding products to branch: {ex.Message}",
                    Data = null
                };
            }

        }

        // UPDATE Method using UpdateBranchProductDto
        public async Task<ApiResponse<BranchProductResponseDto>> UpdateBranchProductAsync(int branchId, int productId, UpdateBranchProductDto updateDto)
        {
            try
            {
                // Find the existing BranchProduct using the composite key (BranchId, ProductId)
                var existingBranchProduct = await _context.BranchProducts
                                    .Include(b => b.Branch)
                                    .Include(s => s.Status)
                                    .Include(st => st.Status)
                                    .Include(p => p.Product)
                                    .Include(c => c.Product!.Category)
                                    .Include(sp => sp.Product!.Supplier)
                    .FirstOrDefaultAsync(bp => bp.BranchId == branchId && bp.ProductId == productId && bp.DeletedAt == null);

                if (existingBranchProduct == null)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "Branch product not found.",
                        Data = null
                    };
                }

                // Ensure that the BranchId and ProductId match the existing record to avoid unauthorized updates
                if (existingBranchProduct.BranchId != branchId || existingBranchProduct.ProductId != productId)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "Branch or Product ID mismatch.",
                        Data = null
                    };
                }

                // Update fields from the DTO
                existingBranchProduct.StatusId = updateDto.ProductStatusId;
                existingBranchProduct.CostPrice = updateDto.CostPrice;
                existingBranchProduct.RetailPrice = updateDto.RetailPrice;
                existingBranchProduct.StockQuantity = updateDto.StockQuantity;
                existingBranchProduct.Remarks = updateDto.Remarks;
                existingBranchProduct.IsActive = updateDto.IsActive;

                // Optionally update the UpdatedBy field to track who made the change
                existingBranchProduct.UpdatedBy = updateDto.UpdatedBy;
                existingBranchProduct.UpdatedAt = DateTime.UtcNow; // Assuming you have an UpdatedAt field

                // Save changes
                await _context.SaveChangesAsync();

                // Map updated BranchProduct to response DTO
                var responseDto = _mapper.Map<BranchProductResponseDto>(existingBranchProduct);

                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = true,
                    Message = "Branch product updated successfully.",
                    Data = responseDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = false,
                    Message = $"Error occurred while updating branch product: {ex.Message}",
                    Data = null
                };
            }
        }

        // Soft DELETE Method
        public async Task<ApiResponse<bool>> SoftDeleteBranchProductAsync(int branchId, int productId)
        {
            try
            {
                // Find the BranchProduct using both BranchId and ProductId
                var branchProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == branchId && bp.ProductId == productId && bp.DeletedAt == null);

                if (branchProduct == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Branch product not found.",
                        Data = false
                    };
                }

                // Perform soft delete by setting DeletedAt to the current time
                branchProduct.DeletedAt = DateTime.UtcNow;

                // Save changes
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Branch product deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error occurred while deleting branch product: {ex.Message}",
                    Data = false
                };
            }
        }


    }
}

