using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;

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

        public async Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync()
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

        public async Task<ApiResponse<Product>> CreateProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new ApiResponse<Product>
                {
                    Success = true,
                    Message = "Product created successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = $"Error creating product: {ex.Message}",
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

                // Update fields
                existingProduct.ProductName = product.ProductName;
                existingProduct.SKU = product.SKU;
                existingProduct.ReorderLevel = product.ReorderLevel;
                existingProduct.MinStockLevel = product.MinStockLevel;
                existingProduct.UpdatedAt = DateTime.Now;
                existingProduct.UpdatedBy = product.UpdatedBy;

                _context.Products.Update(existingProduct);
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

        public async Task<ApiResponse<IEnumerable<GetFilter_Products>>> GetFilterProductsAsync(string? category = null, string? sku = null, string? productName = null)
        {
            try
            {
                // Start with filtering products by DeletedAt and optional filters
                var productQuery = _context.Products
                    .Where(p => p.DeletedAt == null);

                // Apply filters for Category, SKU, ProductStatus, and ProductName if provided
                if (!string.IsNullOrEmpty(category))
                {
                    productQuery = productQuery.Where(p => p.Category!.CategoryName!.Contains(category));
                }

                if (!string.IsNullOrEmpty(sku))
                {
                    productQuery = productQuery.Where(p => p.SKU.Contains(sku));
                }


                if (!string.IsNullOrEmpty(productName))
                {
                    productQuery = productQuery.Where(p => p.ProductName.Contains(productName));
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

        public async Task<ApiResponse<bool>> DeleteProductAsync(int productId, string employeeId)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.Id == productId && p.DeletedAt == null)  // Ensure product isn't already deleted
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Product not found or already deleted.",
                        Data = false
                    };
                }

                // Soft delete the product
                product.DeletedAt = DateTime.Now;
                product.DeletedBy = employeeId;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Product marked as deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Log exception
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error deleting product: {ex.Message}",
                    Data = false
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

        public async Task<ApiResponse<IEnumerable<Product>>> RestoreDeletedProductsAsync(DeletedProductIdsDto deletedProducts)
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

    }
}
