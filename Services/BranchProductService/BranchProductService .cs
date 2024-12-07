using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Services.BranchProductService
{
    public class BranchProductService : IBranchProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchProductService> _logger;


        public BranchProductService(ApplicationDbContext context, IMapper mapper, ILogger<BranchProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId)
        {
            try
            {
                // Fetch the branch to ensure it exists
                var branch = await _context.Branches.FindAsync(branchId);
                if (branch == null)
                {
                    return new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
                    {
                        Success = false,
                        Message = "Branch not found.",
                        Data = null
                    };
                }

                // Fetch the branch products
                var branchProducts = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.DeletedAt == null)
                    .Include(bp => bp.Product)  // Include the Product
                        .ThenInclude(p => p!.Category)  // Include Category related to Product
                    .Include(bp => bp.Product!.Supplier)  // Explicitly Include Supplier related to Product
                    .Include(bp => bp.Status)  // Include Status to avoid null reference issues
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

                // Map the branch products to response DTO
                var responseDtos = branchProducts.Select(bp => new GetAllBranchProductResponseDto
                {
                    BranchId = bp.BranchId,
                    BranchName = bp.Branch!.BranchName,
                    ProductId = bp.Product!.Id,
                    ProductName = bp.Product.ProductName,
                    CategoryId = bp.Product.Category!.Id,
                    Category = bp.Product.Category.CategoryName,
                    SupplierId = bp.Product.Supplier!.Id,  // Get SupplierId from Product
                    Supplier = bp.Product.Supplier.SupplierName,  // Get SupplierName from Product
                    StatusId = bp.StatusId,
                    StatusName = bp.Status!.StatusName,
                    CostPrice = bp.CostPrice,
                    MinStockLevel = bp.MinStockLevel,
                    ReorderLevel = bp.ReorderLevel,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
                    Remarks = bp.Remarks,
                    SKU = bp.Product.SKU,
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
                // Log the exception (optional, depending on logging setup)
                return new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<BranchProductResponseDto>>> GetBranchProductsByPageAsync(
            int branchId,
            int page,
            int pageSize,
            string sortField,
            int sortOrder)
        {
            // Map sortField to an actual Expression<Func<BranchProduct, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            // Filter by branchId
            IQueryable<BranchProduct> query = _context.BranchProducts
                .Include(bp => bp.Product) // Include related Product data if necessary
                .Where(bp => bp.BranchId == branchId);

            // Count total records
            var totalCount = await query.CountAsync();

            // Apply sorting dynamically based on sortOrder
            query = sortOrder == 1
                ? query.OrderBy(sortExpression)
                : query.OrderByDescending(sortExpression);

            // Apply pagination
            var branchProducts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTO
            var branchProductDtos = _mapper.Map<IEnumerable<BranchProductResponseDto>>(branchProducts);

            return new ApiResponse<IEnumerable<BranchProductResponseDto>>
            {
                Success = true,
                Message = "Branch products retrieved successfully.",
                Data = branchProductDtos,
                TotalCount = totalCount
            };
        }

        private Expression<Func<BranchProduct, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "productName":
                    return bp => bp.Product!.ProductName;
                case "status":
                    return bp => bp.Status!;
                case "costPrice":
                    return bp => bp.CostPrice;
                case "retailPrice":
                    return bp => bp.RetailPrice;
                case "stockQuantity":
                    return bp => bp.StockQuantity;
                case "createdAt":
                    return bp => bp.CreatedAt;
                case "minStockLevel":
                    return bp => bp.MinStockLevel;
                case "reorderLevel":
                    return bp => bp.ReorderLevel;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<BranchProductResponseDto>> GetProductsInBranchAsync(int branchId, int productId)
        {
            try
            {
                // Fetch the branch product with specific branchId and productId, and ensure it's not deleted
                var branchProduct = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.ProductId == productId && bp.DeletedAt == null)
                    .Include(bp => bp.Product)
                        .ThenInclude(p => p!.Category)
                    .FirstOrDefaultAsync();

                // If no product found for the given branchId and productId, return an error response
                if (branchProduct == null)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "Product not found for this branch.",
                        Data = null
                    };
                }

                // Fetch branch details using branchId
                var branch = await _context.Branches.FindAsync(branchId);

                // Map the branch product data to a DTO
                var responseDto = new BranchProductResponseDto
                {
                    BranchId = branch!.Id,
                    BranchName = branch.BranchName,
                    ProductId = branchProduct.Product!.Id,
                    StatusId = branchProduct.StatusId,
                    ProductName = branchProduct.Product.ProductName,
                    Category = branchProduct.Product.Category!.CategoryName!,
                    CostPrice = branchProduct.CostPrice,
                    RetailPrice = branchProduct.RetailPrice,
                    StockQuantity = branchProduct.StockQuantity,
                    Remarks = branchProduct.Remarks!,
                    ProductSKU = branchProduct.Product.SKU,
                };

                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = true,
                    Message = "Product retrieved successfully.",
                    Data = responseDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = false,
                    Message = $"Error occurred while fetching the product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<GetBranchProductFilterDto>>> GetFilteredBranchProductsAsync(FilterBranchProductsParams filter)
        {
            try
            {
                // Start with filtering the branch products by branchId and DeletedAt
                var branchProductsQuery = _context.BranchProducts
                    .Where(bp => bp.BranchId == filter.BranchId && bp.DeletedAt == null);

                // Apply the filtering for productCategory if provided
                if (!string.IsNullOrEmpty(filter.ProductCategory))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.Category!.CategoryName!.Contains(filter.ProductCategory));
                }

                // Apply the filtering for SKU if provided
                if (!string.IsNullOrEmpty(filter.Sku))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.SKU.Contains(filter.Sku));
                }

                // Apply the filtering for SKU if provided
                if (!string.IsNullOrEmpty(filter.ProductName))
                {
                    branchProductsQuery = branchProductsQuery.Where(bp => bp.Product!.ProductName.Contains(filter.ProductName));
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

                    // Add an entry to the BranchProductAuditTrail
                    var auditTrail = new BranchProductAuditTrail
                    {
                        BranchId = branchProduct.BranchId,
                        ProductId = branchProduct.ProductId,
                        Action = "Created",
                        ActionDate = DateTime.UtcNow,
                        ActionBy = branchProduct.CreatedBy,
                        NewValue = $"CostPrice: {branchProduct.CostPrice} RetailPrice: {branchProduct.RetailPrice} StockQuantity: {branchProduct.StockQuantity}  ReorderLevel: {branchProduct.ReorderLevel} MinStockLevel: {branchProduct.MinStockLevel}",
                        Remarks = branchProduct.Remarks
                    };


                    _context.BranchProductAuditTrails.Add(auditTrail);
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

                                CategoryId = product.CategoryId,
                                Category = product.Category?.CategoryName ?? "No Category",

                                SupplierId = product.SupplierId,
                                Supplier = product.Supplier?.SupplierName ?? "No Supplier",

                                ProductSKU = product.SKU,

                                StatusId = branchProduct.StatusId,
                                Status = branchProduct.Status?.StatusName ?? "No Status",

                                CostPrice = branchProduct.CostPrice,
                                RetailPrice = branchProduct.RetailPrice,
                                StockQuantity = branchProduct.StockQuantity,
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

        public async Task<ApiResponse<BranchProductResponseDto>> UpdateBranchProductAsync(UpdateBranchProductDto updateDto)
        {
            try
            {
                // Find the existing BranchProduct
                var existingBranchProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == updateDto.BranchId && bp.ProductId == updateDto.ProductId && bp.DeletedAt == null);

                if (existingBranchProduct == null)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "Branch product not found.",
                        Data = null
                    };
                }

                // Create dictionaries for Old and New values using the helper method
                var (newValues, oldValues) = BranchProductAuditTrailHelper.CreateAuditTrailValues(updateDto, existingBranchProduct);

                // Convert dictionaries to JSON strings for saving in the audit trail
                var newValueJson = JsonConvert.SerializeObject(newValues);
                var oldValueJson = JsonConvert.SerializeObject(oldValues);

                // Create the audit trail entry using the helper method
                var auditTrail = BranchProductAuditTrailHelper.CreateAuditTrailEntry(
                    existingBranchProduct,
                    "Updated",
                    updateDto.UpdatedBy!,
                    newValueJson,
                    oldValueJson,
                    updateDto.Remarks!
                );

                // Save the audit trail to the database
                _context.BranchProductAuditTrails.Add(auditTrail);

                // Update the existing branch product with new values
                existingBranchProduct.CostPrice = updateDto.CostPrice;
                existingBranchProduct.RetailPrice = updateDto.RetailPrice;
                existingBranchProduct.StockQuantity = updateDto.StockQuantity;
                existingBranchProduct.MinStockLevel = updateDto.MinStockLevel;
                existingBranchProduct.ReorderLevel = updateDto.ReorderLevel;
                existingBranchProduct.Remarks = updateDto.Remarks;
                existingBranchProduct.UpdatedAt = DateTime.UtcNow;
                existingBranchProduct.UpdatedBy = updateDto.UpdatedBy;

                await _context.SaveChangesAsync();

                // Return response
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
                _logger.LogError(ex, "Error occurred while updating branch product.");
                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = false,
                    Message = $"Error occurred while updating branch product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteBranchProductAsync(SoftDeleteBranchProductParams deleteBranchProduct)
        {
            try
            {
                // Log the start of the soft delete process
                _logger.LogInformation("Starting soft delete for BranchId: {BranchId}, ProductId: {ProductId} by User: {DeletedBy}",
                    deleteBranchProduct.BranchId, deleteBranchProduct.ProductId, deleteBranchProduct.DeletedBy);

                // Find the existing BranchProduct using the composite key (BranchId, ProductId)
                var existingBranchProduct = await _context.BranchProducts
                                            .IgnoreQueryFilters()
                                            .FirstOrDefaultAsync(bp => bp.BranchId == deleteBranchProduct.BranchId && bp.ProductId == deleteBranchProduct.ProductId && bp.DeletedAt == null);

                if (existingBranchProduct == null)
                {
                    _logger.LogWarning("Branch product not found for BranchId: {BranchId}, ProductId: {ProductId}", deleteBranchProduct.BranchId, deleteBranchProduct.ProductId);
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Branch product not found.",
                        Data = false
                    };
                }

                // Mark the BranchProduct as deleted (soft delete)
                existingBranchProduct.DeletedAt = DateTime.UtcNow;
                existingBranchProduct.DeletedBy = deleteBranchProduct.DeletedBy;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Log the successful soft delete
                _logger.LogInformation("Successfully soft deleted BranchProduct for BranchId: {BranchId}, ProductId: {ProductId}", deleteBranchProduct.BranchId, deleteBranchProduct.ProductId);


                // Create an audit trail entry
                var auditTrail = BranchProductAuditTrailHelper.CreateAuditTrailEntry(
                    existingBranchProduct,
                    "SoftDeleted",
                    deleteBranchProduct.DeletedBy!,
                    "Old Value - N/A",
                    "New Value - N/A",
                    "Soft delete action performed"
                );

                _context.BranchProductAuditTrails.Add(auditTrail);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Branch product soft deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while soft deleting BranchProduct for BranchId: {BranchId}, ProductId: {ProductId}", deleteBranchProduct.BranchId, deleteBranchProduct.ProductId);

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error occurred while soft deleting branch product: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<List<BranchProductAuditTrailsDto>>> GetBranchProductAuditTrailsAsync(int branchId, int productId)
        {
            try
            {
                // Fetch product audit trails for the given productId and branchId
                var trails = await _context.BranchProductAuditTrails
                    .IgnoreQueryFilters()
                    .Where(p => p.ProductId == productId && p.BranchId == branchId)  // Use both ProductId and BranchId
                    .ToListAsync();

                // Check if any audit trails were found
                if (trails.Count == 0)
                {
                    return new ApiResponse<List<BranchProductAuditTrailsDto>>
                    {
                        Success = false,
                        Message = "No audit trails found for the specified product and branch.",
                        Data = null
                    };
                }

                // Map the list of ProductAuditTrail entities to the list of ProductAuditTrailsDto
                var auditTrails = _mapper.Map<List<BranchProductAuditTrailsDto>>(trails);

                return new ApiResponse<List<BranchProductAuditTrailsDto>>
                {
                    Success = true,
                    Message = "Audit details retrieved successfully.",
                    Data = auditTrails
                };
            }
            catch (Exception ex)
            {
                // Log exception (optional logging mechanism)
                _logger.LogError(ex, "Error retrieving audit details for product ID {ProductId} and branch ID {BranchId}", productId, branchId);

                return new ApiResponse<List<BranchProductAuditTrailsDto>>
                {
                    Success = false,
                    Message = $"Error retrieving audit details: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<BranchProductDto>>> GetAllDeletedBranchProductsAsync()
        {
            try
            {

                var branchProducts = await _context.BranchProducts
                    .IgnoreQueryFilters()
                    .Where(p => p.DeletedAt != null) // Soft delete filter
                    .Include(bp => bp.Product)
                    .ThenInclude(p => p!.Category)
                    .ToListAsync();

                var branchProductsDto = _mapper.Map<IEnumerable<BranchProductDto>>(branchProducts);

                if (!branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<BranchProductDto>>
                    {
                        Success = false,
                        Message = "No products found.",
                        Data = null
                    };
                }

                return new ApiResponse<IEnumerable<BranchProductDto>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = branchProductsDto
                };
            }
            catch (Exception ex)
            {
                // Log exception (log framework such as Serilog, NLog, etc. can be used here)
                return new ApiResponse<IEnumerable<BranchProductDto>>
                {
                    Success = false,
                    Message = $"Error retrieving products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<int>>> RestoreDeletedBrachProductsAsync(RestoreDeletedBranchProductsParams restoreDeleted)
        {
            var response = new ApiResponse<List<int>>();

            try
            {
                // Validate input parameters
                if (restoreDeleted?.ProductIds == null || !restoreDeleted.ProductIds.Any() ||
                    restoreDeleted.BranchIds == null || !restoreDeleted.BranchIds.Any() || restoreDeleted.RestoredBy == null)

                {
                    response.Success = false;
                    response.Message = "No product or branch IDs provided.";
                    response.Data = null;
                    return response;
                }

                // Fetch deleted products that match the provided ProductIds and BranchIds
                var productsToRestore = await _context.BranchProducts
                    .IgnoreQueryFilters() // Include soft-deleted records
                    .Where(bp => restoreDeleted.ProductIds.Contains(bp.ProductId)
                                 && restoreDeleted.BranchIds.Contains(bp.BranchId)
                                 && bp.DeletedAt != null)
                    .ToListAsync();

                // If no products are found for restoration
                if (!productsToRestore.Any())
                {
                    response.Success = false;
                    response.Message = "No deleted products found for the given IDs and branches.";
                    response.Data = null;
                    return response;
                }

                // Restore the products
                foreach (var branchProduct in productsToRestore)
                {
                    branchProduct.DeletedAt = null; // Mark as restored
                    branchProduct.DeletedBy = null; // Clear deletion metadata

                    // Add an audit trail entry
                    var auditTrail = new BranchProductAuditTrail
                    {
                        ProductId = branchProduct.ProductId,
                        BranchId = branchProduct.BranchId,
                        Action = "Restore",
                        NewValue = $"BranchId: {branchProduct.BranchId}, ProductId: {branchProduct.ProductId}",
                        Remarks = restoreDeleted.Remarks,
                        ActionBy = restoreDeleted.RestoredBy,
                        ActionDate = DateTime.UtcNow
                    };

                    _context.BranchProductAuditTrails.Add(auditTrail);
                }

                // Save changes
                await _context.SaveChangesAsync();

                // Collect the restored product IDs
                var restoredProductIds = productsToRestore.Select(bp => bp.ProductId).ToList();

                // Build response
                response.Success = true;
                response.Message = $"{restoredProductIds.Count} products restored successfully.";
                response.Data = restoredProductIds;

                return response;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while restoring deleted products.");

                // Return error response
                response.Success = false;
                response.Message = $"Error restoring products: {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<ApiResponse<IEnumerable<BranchProductFilterWithCategorySupplierDTO>>> GetBranchProductsByCategoryAndSupplierAsync(int categoryId, int supplierId, int branchId)
        {
            var branchProducts = await _context.BranchProducts
                .Include(bp => bp.Product)
                .ThenInclude(p => p!.Category)
                .Include(bp => bp.Product!.Supplier)
                .Where(bp => bp.Product != null
                             && bp.Product.CategoryId == categoryId
                             && bp.Product.SupplierId == supplierId
                             && bp.BranchId == branchId // Added branchId filter
                             && bp.DeletedAt == null)
                .Select(bp => new BranchProductFilterWithCategorySupplierDTO
                {
                    BranchId = bp.BranchId,
                    ProductId = bp.ProductId,
                    ProductName = bp.Product!.ProductName,
                    SupplierId = bp.Product.SupplierId,
                    SupplierName = bp.Product.Supplier!.SupplierName!,
                    CategoryId = bp.Product.CategoryId,
                    CategoryName = bp.Product.Category!.CategoryName!,
                    SKU = bp.Product.SKU,
                    StockQuantity = bp.StockQuantity
                })
                .ToListAsync();

            if (!branchProducts.Any())
            {
                return new ApiResponse<IEnumerable<BranchProductFilterWithCategorySupplierDTO>>
                {
                    Success = false,
                    Message = "No branch products found for the specified category, supplier, and branch.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<BranchProductFilterWithCategorySupplierDTO>>
            {
                Success = true,
                Message = "Branch products retrieved successfully.",
                Data = branchProducts
            };
        }

    }

}



