using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;

namespace Pacifica.API.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto);
        Task<ApiResponse<EmployeeDto>> RegisterEmployeeAsync(RegisterUserDto registerUser);

        Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
        Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync();
        Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto registerDto);
        Task<ApiResponse<IEnumerable<GetFilter_Employee>>> GetEmployeesByPageAsync(int page, int pageSize, string sortField, int sortOrder);

        /// <summary>
        /// Checks if an employee ID or email exists in the database.
        /// </summary>
        /// <param name="value">The employee ID or email to check.</param>
        /// <param name="type">The type of value ("employeeId" or "email").</param>
        /// <returns>True if the value exists, false otherwise.</returns>
        Task<ApiResponse<bool>> CheckIfExistsAsync(string value, string type);

    }
}
