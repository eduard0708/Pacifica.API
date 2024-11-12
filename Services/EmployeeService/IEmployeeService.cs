using PacificaAPI.Dtos.Admin;

namespace PacificaAPI.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto);
        Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
        Task<ApiResponse<List<EmployeeDto>>> GetAllEmployeesAsync();
        Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, RegisterDto registerDto);
    }
}
