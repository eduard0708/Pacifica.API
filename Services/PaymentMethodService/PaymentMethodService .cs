using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Dtos.PaymentMethod;
using Pacifica.API.Models.Transaction;
using Pacifica.API.Services.PaymentMethodService;

namespace Pacifica.API.Services.BranchService
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PaymentMethod>>> GetAllPaymentMethodsAsync()
        {
            var paymentMethod = await _context.PaymentMethods
                .Where(b => b.DeletedAt == null) // Soft delete filter
                .ToListAsync();

            if (!paymentMethod.Any())
            {
                return new ApiResponse<IEnumerable<PaymentMethod>>
                {
                    Success = false,
                    Message = "No payment menthod found.",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<PaymentMethod>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = paymentMethod
            };
        }

        public async Task<ApiResponse<IEnumerable<PaymentMethod>>> GetPaymentMethodByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {
            // Map sortField to an actual Expression<Func<Branch, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<PaymentMethod>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            var totalCount = await _context.PaymentMethods
                .IgnoreQueryFilters() // Ignore QueryFilters for soft delete    
                .CountAsync();

            // Dynamically order the query based on the sort expression and sort order
            IQueryable<PaymentMethod> query = _context.PaymentMethods
                 .IgnoreQueryFilters();  // Ignore global filters, so we can apply soft delete filter manually

            // Apply sorting dynamically based on sortOrder
            query = sortOrder == 1 ? _context.PaymentMethods.IgnoreQueryFilters().OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

            // Apply pagination
            var branches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<PaymentMethod>>
            {
                Success = true,
                Message = "Branches retrieved successfully.",
                Data = branches,
                TotalCount = totalCount
            };
        }

        private Expression<Func<PaymentMethod, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "paymentMethodName":
                    return x => x.PaymentMethodName!;
                case "description":
                    return x => x.Description!;
                case "createdAt":
                    return x => x.CreatedAt;
                case "isDeleted":
                    return x => x.IsDeleted!;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<PaymentMethod>> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

            if (paymentMethod == null)
            {
                return new ApiResponse<PaymentMethod>
                {
                    Success = false,
                    Message = "Payment Method not found.",
                    Data = null
                };
            }

            return new ApiResponse<PaymentMethod>
            {
                Success = true,
                Message = "Payment Method retrieved successfully.",
                Data = paymentMethod
            };
        }

        public async Task<ApiResponse<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return new ApiResponse<PaymentMethod>
            {
                Success = true,
                Message = "PaymentMethod created successfully.",
                Data = paymentMethod
            };
        }

        public async Task<ApiResponse<PaymentMethod>> UpdatePaymentMethodAsync(int id, PaymentMethod paymentMethod)
        {
            var existingPaymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (existingPaymentMethod == null || existingPaymentMethod.DeletedAt != null)
            {
                return new ApiResponse<PaymentMethod>
                {
                    Success = false,
                    Message = "PaymentMethod not found or already deleted.",
                    Data = null
                };
            }

            existingPaymentMethod.PaymentMethodName = paymentMethod.PaymentMethodName;
            existingPaymentMethod.Description = paymentMethod.Description;
            existingPaymentMethod.IsDeleted = paymentMethod.IsDeleted;
            existingPaymentMethod.UpdatedAt = DateTime.Now;
            existingPaymentMethod.UpdatedBy = paymentMethod.UpdatedBy;

            _context.PaymentMethods.Update(existingPaymentMethod);
            await _context.SaveChangesAsync();

            return new ApiResponse<PaymentMethod>
            {
                Success = true,
                Message = "PaymentMethod updated successfully.",
                Data = existingPaymentMethod
            };
        }

        public async Task<ApiResponse<bool>> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null || paymentMethod.DeletedAt != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "PaymentMethod not found or already deleted.",
                    Data = false
                };
            }

            paymentMethod.DeletedAt = DateTime.Now;
            paymentMethod.IsDeleted = true;
            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "PaymentMethod deleted successfully.",
                Data = true
            };
        }
    }
}
