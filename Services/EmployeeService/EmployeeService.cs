using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Admin;

namespace Pacifica.API.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;

        public EmployeeService(UserManager<Employee> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto)
        {
            var employee = _mapper.Map<Employee>(registerDto);
            var result = await _userManager.CreateAsync(employee, registerDto.Password!);

            if (result.Succeeded)
            {
                return new ApiResponse<EmployeeDto> { Success = true, Message = "Employee created successfully", Data = _mapper.Map<EmployeeDto>(employee) };
            }

            return new ApiResponse<EmployeeDto> { Success = false, Message = "Error creating employee" };
        }

        public async Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(string employeeId)
        {
            // Fetch the employee by ID and include related entities (Roles, Department, Position)
            var employee = await _userManager.Users
                .Include(e => e.Roles)   // Include the roles for the employee
                .Include(e => e.Department) // Assuming Department is related to Employee
                .Include(e => e.Position)  // Assuming Position is related to Employee
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                return new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Message = "Employee not found"
                };
            }

            // Map the employee to EmployeeDto using AutoMapper
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new ApiResponse<EmployeeDto>
            {
                Success = true,
                Data = employeeDto
            };
        }
        
        public async Task<ApiResponse<List<EmployeeDto>>> GetAllEmployeesAsync()
        {
            // Get all employees with roles, department, and position
            var employees = await _userManager.Users
                .Include(e => e.Roles) // Include the roles for each employee
                .Include(e => e.Department) // Assuming Department is an entity linked to Employee
                .Include(e => e.Position) // Assuming Position is an entity linked to Employee
                .ToListAsync();

            // Map the Employee entities to EmployeeDto
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

            // Return the response with the mapped employee data
            return new ApiResponse<List<EmployeeDto>>
            {
                Success = true,
                Data = employeeDtos
            };
        }
        public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, RegisterDto registerDto)
        {
            var employee = await _userManager.FindByIdAsync(employeeId);
            if (employee == null) return new ApiResponse<EmployeeDto> { Success = false, Message = "Employee not found" };

            employee = _mapper.Map(registerDto, employee);
            var result = await _userManager.UpdateAsync(employee);

            if (result.Succeeded)
            {
                return new ApiResponse<EmployeeDto> { Success = true, Data = _mapper.Map<EmployeeDto>(employee) };
            }

            return new ApiResponse<EmployeeDto> { Success = false, Message = "Error updating employee" };
        }


    }

}
