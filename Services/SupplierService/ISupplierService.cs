using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.SupplierService
{
    public interface ISupplierService
    {
        Task<ApiResponse<IEnumerable<Supplier>>> GetAllSuppliersAsync();
        Task<ApiResponse<Supplier>> GetSupplierByIdAsync(int id);
        Task<ApiResponse<Supplier>> CreateSupplierAsync(Supplier supplier);
        Task<ApiResponse<Supplier>> UpdateSupplierAsync(int id, Supplier supplier);
        Task<ApiResponse<bool>> DeleteSupplierAsync(int id);
    }
}