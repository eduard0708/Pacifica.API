using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;

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
                    ProductName = bp.Product.ProductName,
                    ProductCategory = bp.Product.Category!.CategoryName,
                    CostPrice = bp.CostPrice,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
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
        public async Task<ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>> GetProductsFilteredByBranchAsync(int branchId, string? productCategory = null, string? sku = null)
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

                // Now, include the related entities after applying filters

                branchProductsQuery = branchProductsQuery.Include(b => b.Branch)
                                                            .Include(ps => ps.ProductStatus)
                                                            .Include(bp => bp.Product)
                                                            .ThenInclude(p => p!.Category);

                // Fetch the filtered products
                var branchProducts = await branchProductsQuery.ToListAsync();

                if (!branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>
                    {
                        Success = false,
                        Message = "No products found matching the filter criteria.",
                        Data = null
                    };
                }

                var responseDtos = branchProducts.Select(bp => new GetBranchProductFilterSupplierCategorySKUDto
                {
                    Branch = new BranchProduct_BranchDto
                    {
                        Id = bp.Branch != null ? bp.Branch.Id : 0,
                        Name = bp.Branch != null ? bp.Branch.BranchName : "Unknown",
                        BranchLocation = bp.Branch!.BranchLocation ?? "Unknown"
                    },

                    Product = new BranchProduct_ProductDto
                    {
                        Id = bp.Product != null ? bp.Product.Id : 0,
                        Name = bp.Product?.ProductName ?? "Unknown",
                        Category = new BranchProduct_CategoryDto
                        {
                            Id = bp.Product?.Category?.Id ?? 0,
                            Category = bp.Product?.Category?.CategoryName ?? "Unknown",
                            Description = bp.Product?.Category?.Description ?? "Unknown"
                        },
                        Status = new BranchProduct_StatusDto
                        {
                            Id = bp.ProductStatus?.Id ?? 0,
                            Status = bp.ProductStatus?.ProductStatusName ?? "Unknown",
                            Description = bp.ProductStatus?.Description ?? "Unknown"
                        },
                        SKU = bp.Product?.SKU ?? "Unknown"
                    },

                    CostPrice = bp.CostPrice,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
                    IsActive = bp.IsActive
                    
                }).ToList();

                return new ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>
                {
                    Success = true,
                    Message = "Filtered products retrieved successfully.",
                    Data = responseDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching filtered products: {ex.Message}",
                    Data = null
                };
            }
        }

        // Existing method to add product to branch
        public async Task<ApiResponse<BranchProductResponseDto>> AddProductToBranchAsync(BranchProduct branchProductDto)
        {
            try
            {
                var branchProduct = _mapper.Map<BranchProduct>(branchProductDto);

                var productExists = await _context.Products
                    .AnyAsync(p => p.Id == branchProduct.ProductId && p.DeletedAt == null);

                if (!productExists)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "The product does not exist.",
                        Data = null
                    };
                }

                var existingProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == branchProduct.BranchId && bp.ProductId == branchProduct.ProductId && bp.DeletedAt == null);

                if (existingProduct != null)
                {
                    return new ApiResponse<BranchProductResponseDto>
                    {
                        Success = false,
                        Message = "This product already exists in the branch.",
                        Data = null
                    };
                }

                _context.BranchProducts.Add(branchProduct);
                await _context.SaveChangesAsync();

                var branch = await _context.Branches.FindAsync(branchProduct.BranchId);
                var product = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == branchProduct.ProductId);

                var productDto = _mapper.Map<ProductDto>(product);

                var responseDto = new BranchProductResponseDto
                {
                    BranchId = branch!.Id,
                    BranchName = branch.BranchName,
                    ProductId = productDto.Id,
                    ProductName = productDto.ProductName,
                    ProductCategory = product!.Category!.CategoryName,
                    CostPrice = branchProduct.CostPrice,
                    RetailPrice = branchProduct.RetailPrice,
                    StockQuantity = branchProduct.StockQuantity,
                    IsActive = branchProduct.IsActive
                };

                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = true,
                    Message = "Product added successfully.",
                    Data = responseDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = false,
                    Message = $"Error occurred while adding product to branch: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
