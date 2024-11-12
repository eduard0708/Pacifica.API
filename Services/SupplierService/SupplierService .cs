using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Models;
using PacificaAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                existingSupplier.IsActive = supplier.IsActive;
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
