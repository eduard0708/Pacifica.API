using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Models; // Assuming you have a Models namespace

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
                // existingProduct.CostPrice = product.CostPrice;
                // existingProduct.RetailPrice = product.RetailPrice;
                // existingProduct.StockQuantity = product.StockQuantity;
                 existingProduct.SKU = product.SKU;
                existingProduct.ReorderLevel = product.ReorderLevel;
                existingProduct.MinStockLevel = product.MinStockLevel;
                existingProduct.ProductStatus = product.ProductStatus;
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

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null || product.DeletedAt != null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Product not found or already deleted.",
                        Data = false
                    };
                }

                product.DeletedAt = DateTime.Now;  // Soft delete
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Product deleted successfully.",
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
    }
}
