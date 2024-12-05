using Pacifica.API.Dtos.Supplier;

namespace Pacifica.API.Services.SupplierService
{
    public interface ISupplierService
    {
        Task<ApiResponse<IEnumerable<Supplier>>> GetAllSuppliersAsync();
        Task<ApiResponse<IEnumerable<Supplier>>> GetSuppliersByPageAsync(int page, int pageSize, string sortField, int sortOrder);
        Task<ApiResponse<Supplier>> GetSupplierByIdAsync(int id);
        Task<ApiResponse<Supplier>> CreateSupplierAsync(Supplier supplier);
        Task<ApiResponse<Supplier>> UpdateSupplierAsync(int id, Supplier supplier);
        Task<ApiResponse<bool>> DeleteSupplierAsync(int id);
    }
}