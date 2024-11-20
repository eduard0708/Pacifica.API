using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<Product>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.DeletedAt == null) // Soft delete filter
                    .Include(p => p.Category) // Include related Category
                    .Include(p => p.Supplier) // Include related Supplier
                    .ToListAsync();

                if (!products.Any())
                {
                    return new ApiResponse<List<Product>>
                    {
                        Success = false,
                        Message = "No products found.",
                        Data = null
                    };
                }

                return new ApiResponse<List<Product>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = products
                };
            }
            catch (Exception ex)
            {
                // Log exception (log framework such as Serilog, NLog, etc. can be used here)
                return new ApiResponse<List<Product>>
                {
                    Success = false,
                    Message = $"Error retrieving products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetAllDeletedProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .IgnoreQueryFilters() // This disables all global query filters, including the DeletedAt check
                    .Where(p => p.DeletedAt != null) // Soft delete filter
                    .Include(p => p.Category) // Include related Categoryyy
                    .Include(p => p.Supplier) // Include related Supplier
                    .ToListAsync();

                if (!products.Any())
                {
                    return new ApiResponse<IEnumerable<Product>>
                    {
                        Success = false,
                        Message = "No products found.",
                        Data = null
                    };
                }

                return new ApiResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = products
                };
            }
            catch (Exception ex)
            {
                // Log exception (log framework such as Serilog, NLog, etc. can be used here)
                return new ApiResponse<IEnumerable<Product>>
                {
                    Success = false,
                    Message = $"Error retrieving products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Product>> GetDeletedProductByIdAsync(int productId)
        {
            try
            {
                // Query for a soft-deleted product by productId, including related Category and Supplier
                var product = await _context.Products
                    .IgnoreQueryFilters() // Disable all global query filters (like the soft delete filter)
                    .Where(p => p.DeletedAt != null && p.Id == productId) // Only deleted products with matching productId
                    .Include(p => p.Category)  // Include related Category
                    .Include(p => p.Supplier)  // Include related Supplier
                    .FirstOrDefaultAsync(); // Retrieve a single product or null

                // If no product found
                if (product == null)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "No deleted product found.",
                        Data = null
                    };
                }

                return new ApiResponse<Product>
                {
                    Success = true,
                    Message = "Deleted product retrieved successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                // Log exception (use a logging framework here)
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = $"Error retrieving deleted product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Product>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

                if (product == null)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Product not found.",
                        Data = null
                    };
                }

                return new ApiResponse<Product>
                {
                    Success = true,
                    Message = "Product retrieved successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = $"Error retrieving product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ProductDto>>> CreateProductsAsync(List<CreateProductDto> productDtos)
        {
            try
            {
                var products = _mapper.Map<List<Product>>(productDtos);  // Map list of DTOs to a list of Product entities

                foreach (var product in products)
                {
                    // Add the product to the database
                    _context.Products.Add(product);
                }

                await _context.SaveChangesAsync();  // Save all products in one go

                // Log the creation in ProductAuditTrail for each product
                var auditTrails = new List<ProductAuditTrail>();

                foreach (var product in products)
                {
                    var auditTrail = new ProductAuditTrail
                    {
                        ProductId = product.Id,
                        Action = "Created",
                        NewValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}",
                        ActionBy = product.CreatedBy,
                        ActionDate = DateTime.Now
                    };

                    auditTrails.Add(auditTrail);  // Add the audit trail for each product
                }

                _context.ProductAuditTrails.AddRange(auditTrails);  // Bulk insert audit trails
                await _context.SaveChangesAsync();  // Save the audit trails

                // Map the created products to DTOs
                var createdProducts = _mapper.Map<List<ProductDto>>(products);

                // Return success response with the list of created product DTOs
                return new ApiResponse<List<ProductDto>>
                {
                    Success = true,
                    Message = "Products created successfully.",
                    Data = createdProducts
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message = $"Error creating products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Product>> UpdateProductAsync(int id, Product product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null || existingProduct.DeletedAt != null)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Product not found or already deleted.",
                        Data = null
                    };
                }

                // Save original values for audit trail
                var originalProductName = existingProduct.ProductName;
                var originalSKU = existingProduct.SKU;
                var originalCategoryId = existingProduct.CategoryId;
                var originalSupplierId = existingProduct.SupplierId;

                // Update fields
                existingProduct.ProductName = product.ProductName;
                existingProduct.SKU = product.SKU;
                existingProduct.UpdatedAt = DateTime.Now;
                existingProduct.UpdatedBy = product.UpdatedBy;

                _context.Products.Update(existingProduct);

                // Create audit trail with old and new values
                var auditTrail = new ProductAuditTrail
                {
                    ProductId = existingProduct.Id, // Use the actual ID from the database
                    Action = "Updated",
                    OldValue = $"ProductName: {originalProductName}, SKU: {originalSKU}, CategoryId: {originalCategoryId}, SupplierId: {originalSupplierId}",
                    NewValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}, CategoryId: {product.CategoryId}, SupplierId: {product.SupplierId}",
                    ActionBy = product.UpdatedBy,
                    Remarks = product.Remarks,
                    ActionDate = DateTime.Now
                };

                _context.ProductAuditTrails.Add(auditTrail);

                await _context.SaveChangesAsync();

                return new ApiResponse<Product>
                {
                    Success = true,
                    Message = "Product updated successfully.",
                    Data = existingProduct
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = $"Error updating product: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<GetFilter_Products>>> GetFilterProductsAsync(ProductFilterParams productFilter)
        {
            try
            {
                // Start with filtering products by DeletedAt and optional filters
                var productQuery = _context.Products
                    .Where(p => p.DeletedAt == null);

                // Apply filters for Category, SKU, ProductStatus, and ProductName if provided
                if (!string.IsNullOrEmpty(productFilter.Category))
                {
                    productQuery = productQuery.Where(p => p.Category!.CategoryName!.Contains(productFilter.Category));
                }

                if (!string.IsNullOrEmpty(productFilter.SKU))
                {
                    productQuery = productQuery.Where(p => p.SKU.Contains(productFilter.SKU));
                }


                if (!string.IsNullOrEmpty(productFilter.ProductName))
                {
                    productQuery = productQuery.Where(p => p.ProductName.Contains(productFilter.ProductName));
                }

                productQuery = productQuery
                    .Include(p => p.Category)
                    .Include(p => p.Supplier);

                // Fetch the filtered products
                var products = await productQuery.ToListAsync();

                if (!products.Any())
                {
                    return new ApiResponse<IEnumerable<GetFilter_Products>>
                    {
                        Success = false,
                        Message = "No products found matching the filter criteria.",
                        Data = null
                    };
                }

                // Map to Product_ProductDto
                var responseDtos = products.Select(p => new GetFilter_Products
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    SKU = p.SKU,
                    Category = new Product_CategoryDto
                    {
                        Id = p.CategoryId,
                        Category = p.Category?.CategoryName ?? "No Category",
                        Description = p.Category?.Description ?? "No Description"
                    },

                }).ToList();

                return new ApiResponse<IEnumerable<GetFilter_Products>>
                {
                    Success = true,
                    Message = "Filtered products retrieved successfully.",
                    Data = responseDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GetFilter_Products>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching filtered products: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<AuditDetails>>> GetProductAuditDetailsAsync(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.Id == productId)
                    .Select(p => new AuditDetails
                    {
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedAt = p.UpdatedAt,
                        UpdatedBy = p.UpdatedBy,
                        DeletedAt = p.DeletedAt,
                        DeletedBy = p.DeletedBy,

                    })
                    .ToListAsync();

                if (product == null)
                {
                    return new ApiResponse<List<AuditDetails>>
                    {
                        Success = false,
                        Message = "Product not found.",
                        Data = null
                    };
                }

                return new ApiResponse<List<AuditDetails>>
                {
                    Success = true,
                    Message = "Audit details retrieved successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<List<AuditDetails>>
                {
                    Success = false,
                    Message = $"Error retrieving audit details: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<Product>>> RestoreDeletedProductsAsync(RestoreDeletedProductsParam deletedProducts)
        {
            var response = new ApiResponse<IEnumerable<Product>>();

            try
            {
                // Make sure ProductIds is not null or empty
                if (deletedProducts?.ProductIds == null || !deletedProducts.ProductIds.Any())
                {
                    response.Success = false;
                    response.Message = "No product IDs provided.";
                    response.Data = null;
                    return response;
                }

                // Fetch the deleted products (ignore global query filters)
                var productsToRestore = await _context.Products
                    .IgnoreQueryFilters() // Disable global query filters
                    .Where(p => deletedProducts.ProductIds.Contains(p.Id) && p.DeletedAt != null)
                    .ToListAsync();

                if (productsToRestore.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No deleted products found for restoration.";
                    response.Data = null;
                    return response;
                }

                // Restore the products (set DeletedAt to null)
                foreach (var product in productsToRestore)
                {
                    product.DeletedAt = null; // Restore by setting DeletedAt to null
                    product.DeletedBy = null; // Optionally clear DeletedBy if necessary

                    // Create the audit trail for the deleted product
                    var auditTrail = new ProductAuditTrail
                    {
                        ProductId = product.Id,
                        Action = "Restore",
                        NewValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}",
                        Remarks = deletedProducts.Remarks,
                        ActionBy = deletedProducts.RestoredBy,
                        ActionDate = DateTime.Now
                    };

                    // Add the audit trail to the context
                    _context.ProductAuditTrails.Add(auditTrail);
                }

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = $"{productsToRestore.Count} products restored successfully.";
                response.Data = productsToRestore;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error restoring products: {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductsAsync(DeletedProductsParam productsDelete)
        {
            try
            {
                // Ensure the ProductIds list is not null or empty
                if (productsDelete.ProductIds == null || !productsDelete.ProductIds.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "No products found to delete.",
                        Data = false
                    };
                }

                // The employee ID who is performing the deletion
                var deletedBy = productsDelete.DeletedBy;

                // Loop through each productId and perform the soft delete
                foreach (var productId in productsDelete.ProductIds)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                    if (product != null)
                    {
                        // Perform the soft delete
                        product.DeletedAt = DateTime.Now;
                        product.DeletedBy = deletedBy; // Associate the deletion with the employee

                        // Update the product in the database
                        _context.Products.Update(product);

                        // Create the audit trail for the deleted product
                        var auditTrail = new ProductAuditTrail
                        {
                            ProductId = product.Id,
                            Action = "Deleted",
                            NewValue = $"ProductName: {product.ProductName}, SKU: {product.SKU}",
                            Remarks = productsDelete.Remarks,
                            ActionBy = deletedBy,
                            ActionDate = DateTime.Now
                        };

                        // Add the audit trail to the context
                        _context.ProductAuditTrails.Add(auditTrail);
                    }
                }

                // Save changes to the database (this will save both product updates and audit trails)
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Products successfully deleted.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting products: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<List<ProductAuditTrailsDto>>> GetProductAuditTrailsAsync(int productId)
        {
            try
            {
                // Fetch product audit trails for the given productId
                var productTrails = await _context.ProductAuditTrails
                    .Where(p => p.ProductId == productId)
                    .ToListAsync();

                // Check if any audit trails were found
                if (productTrails == null || productTrails.Count == 0)
                {
                    return new ApiResponse<List<ProductAuditTrailsDto>>
                    {
                        Success = false,
                        Message = "No audit trails found for the specified product.",
                        Data = null
                    };
                }

                // Map the list of ProductAuditTrail entities to the list of ProductAuditTrailsDto
                var auditTrails = _mapper.Map<List<ProductAuditTrailsDto>>(productTrails);

                return new ApiResponse<List<ProductAuditTrailsDto>>
                {
                    Success = true,
                    Message = "Audit details retrieved successfully.",
                    Data = auditTrails
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<List<ProductAuditTrailsDto>>
                {
                    Success = false,
                    Message = $"Error retrieving audit details: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
