using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;

namespace Pacifica.API.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto);
        Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
        Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync();
        Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto registerDto);
        Task<ApiResponse<IEnumerable<GetFilter_Employee>>> GetEmployeesByPageAsync(int page, int pageSize, string sortField, int sortOrder);

    }
}
