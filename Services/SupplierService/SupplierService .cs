using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Category;
using Pacifica.API.Dtos.Supplier;

namespace Pacifica.API.Services.SupplierService
{

    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SupplierService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<Supplier>>> GetAllSuppliersAsync()
        {
            try
            {
                var suppliers = await _context.Suppliers
                    .Where(s => s.DeletedAt == null)  // Soft delete check
                    .ToListAsync();

                if (suppliers == null || !suppliers.Any())
                {
                    return new ApiResponse<IEnumerable<Supplier>>
                    {
                        Success = false,
                        Message = "No suppliers found.",
                        Data = null
                    };
                }

                return new ApiResponse<IEnumerable<Supplier>>
                {
                    Success = true,
                    Message = "Suppliers retrieved successfully.",
                    Data = suppliers
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<Supplier>>
                {
                    Success = false,
                    Message = $"Error occurred while fetching suppliers: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SelectSupplierDTO>>> GetSelectSuppliersAsync(int id)
        {
            try
            {
                var suppliers = await _context.Products
                    .Where(p => p.DeletedAt == null && p.SupplierId == id) // Add the correct condition
                    .Include(p => p.Supplier) // Ensure the Supplier navigation property is loaded
                    .Select(p => new SelectSupplierDTO
                    {
                        Id = p.Id,
                        SupplierName = p.Supplier!.SupplierName // Access Supplier's name
                    })
                    .ToListAsync();

                if (suppliers == null || !suppliers.Any())
                {
                    return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                    {
                        Success = false,
                        Message = "No suppliers found.",
                        Data = null
                    };
                }

                return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                {
                    Success = true,
                    Message = "Suppliers retrieved successfully.",
                    Data = suppliers
                };
            }
            catch (Exception ex)
            {
                // Handle exception and log if necessary
                return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SelectSupplierDTO>>> GetSuppliersByCategory(int categoryId)
        {
            try
            {
                var suppliers = await _context.Products
                    .Where(c => c.DeletedAt == null && c.CategoryId == categoryId) // Filter products by category and deletion status
                    .Select(p => p.Supplier)  // Select the Supplier associated with each product
                    .Distinct()
                    .ToListAsync();

                if (suppliers == null || !suppliers.Any())
                {
                    return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                    {
                        Success = false,
                        Message = "No suppliers found.",
                        Data = null
                    };
                }

                // Use AutoMapper to map the supplier list to SelectSupplierDTO
                var supplierDtos = _mapper.Map<List<SelectSupplierDTO>>(suppliers);

                return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                {
                    Success = true,
                    Message = "Suppliers found.",
                    Data = supplierDtos
                };
            }
            catch (Exception)
            {
                return new ApiResponse<IEnumerable<SelectSupplierDTO>>
                {
                    Success = false,
                    Message = "An error occurred while fetching suppliers.",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<IEnumerable<Supplier>>> GetSuppliersByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {
            // Map sortField to an actual Expression<Func<Branch, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<Supplier>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            var totalCount = await _context.Suppliers
                .IgnoreQueryFilters() // Ignore QueryFilters for soft delete    
                .CountAsync();

            // Dynamically order the query based on the sort expression and sort order
            IQueryable<Supplier> query = _context.Suppliers
                 .IgnoreQueryFilters();  // Ignore global filters, so we can apply soft delete filter manually

            // Apply sorting dynamically based on sortOrder
            query = sortOrder == 1 ? _context.Suppliers.IgnoreQueryFilters().OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

            // Apply pagination
            var branches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<IEnumerable<Supplier>>
            {
                Success = true,
                Message = "Supplier retrieved successfully.",
                Data = branches,
                TotalCount = totalCount
            };
        }

        private Expression<Func<Supplier, object>> GetSortExpression(string sortField)
        {
            switch (sortField)
            {
                case "supplierName":
                    return x => x.SupplierName!;
                case "contactPerson":
                    return x => x.ContactPerson!;
                case "contactNumber":
                    return x => x.ContactNumber!;
                case "isDeleted":
                    return x => x.IsDeleted!;
                default:
                    return null!;
            }
        }

        public async Task<ApiResponse<Supplier>> GetSupplierByIdAsync(int id)
        {
            try
            {
                var supplier = await _context.Suppliers
                    .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

                if (supplier == null)
                {
                    return new ApiResponse<Supplier>
                    {
                        Success = false,
                        Message = "Supplier not found.",
                        Data = null
                    };
                }

                return new ApiResponse<Supplier>
                {
                    Success = true,
                    Message = "Supplier retrieved successfully.",
                    Data = supplier
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Supplier>
                {
                    Success = false,
                    Message = $"Error occurred while fetching supplier: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Supplier>> CreateSupplierAsync(Supplier supplier)
        {

            try
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                return new ApiResponse<Supplier>
                {
                    Success = true,
                    Message = "Supplier created successfully.",
                    Data = supplier
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Supplier>
                {
                    Success = false,
                    Message = $"Error occurred while creating supplier: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Supplier>> UpdateSupplierAsync(int id, Supplier supplier)
        {
            try
            {
                var existingSupplier = await _context.Suppliers.FindAsync(id);
                if (existingSupplier == null || existingSupplier.DeletedAt != null)
                {
                    return new ApiResponse<Supplier>
                    {
                        Success = false,
                        Message = "Supplier not found or already deleted.",
                        Data = null
                    };
                }

                existingSupplier.SupplierName = supplier.SupplierName;
                existingSupplier.ContactPerson = supplier.ContactPerson;
                existingSupplier.ContactNumber = supplier.ContactNumber;
                existingSupplier.IsDeleted = supplier.IsDeleted;
                existingSupplier.UpdatedAt = DateTime.Now;
                existingSupplier.UpdatedBy = supplier.UpdatedBy;

                _context.Suppliers.Update(existingSupplier);
                await _context.SaveChangesAsync();

                return new ApiResponse<Supplier>
                {
                    Success = true,
                    Message = "Supplier updated successfully.",
                    Data = existingSupplier
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Supplier>
                {
                    Success = false,
                    Message = $"Error occurred while updating supplier: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteSupplierAsync(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null || supplier.DeletedAt != null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Supplier not found or already deleted.",
                        Data = false
                    };
                }

                supplier.DeletedAt = DateTime.Now;  // Soft delete
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Supplier deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error occurred while deleting supplier: {ex.Message}",
                    Data = false
                };
            }
        }

    }
}
