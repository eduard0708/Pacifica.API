using Pacifica.API.Dtos.UserManagement;
using Pacifica.API.Helper;
using Pacifica.API.Models.EmployeManagement;

namespace Pacifica.API.Services.EmployeeManagementService
{
    public interface IDepartmentService
    {
        Task<ApiResponse<IEnumerable<Department>>> GetAllAsync();
        Task<ApiResponse<Department?>> GetByIdAsync(int id);
        Task<ApiResponse<Department>> CreateAsync(DepartmentDto departmentDto);
        Task<ApiResponse<Department?>> UpdateAsync(int id, DepartmentDto departmentDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
