using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
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

        // Get all products for a specific branch
        public async Task<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>> GetAllProductsByBranchAsync(int branchId)
        {
            try
            {
                var branchProducts = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.DeletedAt == null)
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

                var listBranchProducts = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId)  // Filter by branch ID
                    .Include(bp => bp.Product)             // Include Product
                        .ThenInclude(p => p.Category)      // Include Category within Product
                    .ToListAsync();

                var branch = await _context.Branches.FindAsync(branchId);

                var responseDtos = listBranchProducts.Select(bp => new GetAllBranchProductResponseDto
                {
                    BranchId = branch!.Id,
                    BranchName = branch.BranchName,
                    ProductId = bp.Product!.Id,
                    ProductName = bp.Product.ProductName,
                    ProductCategory = bp.Product.Category!.CategoryName,
                    CostPrice = bp.CostPrice,
                    RetailPrice = bp.RetailPrice,
                    StockQuantity = bp.StockQuantity,
                    SKU = bp.SKU,
                    IsActive = bp.IsActive
                }).ToList();

                // var branch = await _context.Branches.FindAsync(branchId);
                // var product = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == branchId);
                // var branchProduct = await _context.BranchProducts.FindAsync(branchId);

                // var responseDto = new GetAllBranchProductResponseDto
                // {
                //     BranchId = branch!.Id,
                //     BranchName = branch.BranchName,
                //     ProductId = product!.Id,  // Include full product details
                //     ProductName = product.ProductName,  // Include full product details
                //     ProductCategory = product!.Category!.CategoryName,
                //     CostPrice = branchProduct!.CostPrice,
                //     RetailPrice = branchProduct.RetailPrice,
                //     StockQuantity = branchProduct.StockQuantity,
                //     SKU = branchProduct.SKU,
                //     IsActive = branchProduct.IsActive
                // };

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
                    ProductId = productDto.Id,  // Include full product details
                    ProductName = productDto.ProductName,  // Include full product details
                    ProductCategory = product!.Category!.CategoryName,
                    CostPrice = branchProduct.CostPrice,
                    RetailPrice = branchProduct.RetailPrice,
                    StockQuantity = branchProduct.StockQuantity,
                    SKU = branchProduct.SKU,
                    IsActive = branchProduct.IsActive
                };

                return new ApiResponse<BranchProductResponseDto>
                {
                    Success = true,
                    Message = "Success",
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

