using Pacifica.API.Dtos.Admin;

namespace Pacifica.API.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto);
        Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
        Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync();
        Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto registerDto);
    }
}
