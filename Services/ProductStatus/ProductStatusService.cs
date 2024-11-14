using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.ProductStatusService
{
    public class ProductStatusService : IProductStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductStatusService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductStatus>>> GetAllProductStatusesAsync()
        {
            var productStatus = await _context.ProductStatuses
                .Where(tr => tr.DeletedAt == null)
                .ToListAsync();

            if (!productStatus.Any())
            {
                return new ApiResponse<IEnumerable<ProductStatus>>
                {
                    Success = false,
                    Message = "No transaction references found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<ProductStatus>>
            {
                Success = true,
                Message = "Transaction references retrieved successfully.",
                Data = productStatus
            };
        }

        public async Task<ApiResponse<ProductStatus>> GetProductStatusByIdAsync(int id)
        {
            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(tr => tr.Id == id && tr.DeletedAt == null);

            if (productStatus == null)
            {
                return new ApiResponse<ProductStatus>
                {
                    Success = false,
                    Message = "Transaction reference not found.",
                    Data = null
                };
            }

            return new ApiResponse<ProductStatus>
            {
                Success = true,
                Message = "Transaction reference retrieved successfully.",
                Data = productStatus
            };
        }

        public async Task<ApiResponse<ProductStatus>> CreateProductStatusAsync(ProductStatus productStatus)
        {
            _context.ProductStatuses.Add(productStatus);
            await _context.SaveChangesAsync();

            return new ApiResponse<ProductStatus>
            {
                Success = true,
                Message = "Transaction reference created successfully.",
                Data = productStatus
            };
        }

        public async Task<ApiResponse<ProductStatus>> UpdateProductStatusAsync(int id, ProductStatus productStatus)
        {
            var existingProductStatus = await _context.ProductStatuses.FindAsync(id);
            if (existingProductStatus == null || existingProductStatus.DeletedAt != null)
            {
                return new ApiResponse<ProductStatus>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = null
                };
            }

            existingProductStatus.ProductStatusName = productStatus.ProductStatusName;
            existingProductStatus.Description = productStatus.Description;
            existingProductStatus.UpdatedAt = DateTime.Now;
            existingProductStatus.UpdatedBy = productStatus.UpdatedBy;

            _context.ProductStatuses.Update(existingProductStatus);
            await _context.SaveChangesAsync();

            return new ApiResponse<ProductStatus>
            {
                Success = true,
                Message = "Transaction reference updated successfully.",
                Data = existingProductStatus
            };
        }

        public async Task<ApiResponse<bool>> DeleteProductStatusAsync(int id)
        {
            var productStatus = await _context.TransactionTypes.FindAsync(id);
            if (productStatus == null || productStatus.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Transaction reference not found or already deleted.",
                    Data = false
                };
            }

            productStatus.DeletedAt = DateTime.Now;
            _context.TransactionTypes.Update(productStatus);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Transaction reference deleted successfully.",
                Data = true
            };
        }
    }
}






