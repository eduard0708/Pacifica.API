using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Models;  // Assuming your models or DTOs are in the Models namespace

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
        public async Task<ApiResponse<IEnumerable<BranchProduct>>> GetAllProductsByBranchAsync(int branchId)
        {
            try
            {
                var branchProducts = await _context.BranchProducts
                    .Where(bp => bp.BranchId == branchId && bp.DeletedAt == null)
                    .ToListAsync();

                if (!branchProducts.Any())
                {
                    return new ApiResponse<IEnumerable<BranchProduct>>
                    {
                        Success = false,
                        Message = "No products found for this branch.",
                        Data = null
                    };
                }

                return new ApiResponse<IEnumerable<BranchProduct>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = branchProducts
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<BranchProduct>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching products: {ex.Message}",
                    Data = null
                };
            }
        }

        // Add a product to the branch
        public async Task<ApiResponse<BranchProduct>> AddProductToBranchAsync(BranchProduct branchProductDto)
        {
            try
            {
                // Map the incoming DTO to the BranchProduct entity using AutoMapper
                var branchProduct = _mapper.Map<BranchProduct>(branchProductDto);

                // Check if the product exists in the Product table
                var productExists = await _context.Products
                    .AnyAsync(p => p.Id == branchProduct.ProductId && p.DeletedAt == null);

                if (!productExists)
                {
                    return new ApiResponse<BranchProduct>
                    {
                        Success = false,
                        Message = "The product does not exist.",
                        Data = null
                    };
                }

                // Check if product already exists in the branch
                var existingProduct = await _context.BranchProducts
                    .FirstOrDefaultAsync(bp => bp.BranchId == branchProduct.BranchId && bp.ProductId == branchProduct.ProductId && bp.DeletedAt == null);

                if (existingProduct != null)
                {
                    return new ApiResponse<BranchProduct>
                    {
                        Success = false,
                        Message = "This product already exists in the branch.",
                        Data = null
                    };
                }

                // Add the new product to the branch
                _context.BranchProducts.Add(branchProduct);
                await _context.SaveChangesAsync();

                return new ApiResponse<BranchProduct>
                {
                    Success = true,
                    Message = "Product added to branch successfully.",
                    Data = branchProduct
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BranchProduct>
                {
                    Success = false,
                    Message = $"Error occurred while adding product to branch: {ex.Message}",
                    Data = null
                };
            }
        }

       
    }
}
