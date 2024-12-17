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
        private readonly ApplicationDbContext _context;

        public EmployeeService(UserManager<Employee> userManager, IMapper mapper, ApplicationDbContext context) 
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(RegisterDto registerDto)
        {
            // Map the RegisterDto to Employee
            var employee = _mapper.Map<Employee>(registerDto);

            // Handle Department relationship
            if (registerDto.DepartmentId != null)
            {
                var department = await _context.Departments.FindAsync(registerDto.DepartmentId);
                if (department != null)
                {
                    employee.Department = department;  // Set the navigation property, not just the Id
                }
            }

            // Handle Position relationship
            if (registerDto.PositionId.HasValue)
            {
                var position = await _context.Positions.FindAsync(registerDto.PositionId);
                if (position != null)
                {
                    employee.Position = position;  // Set the navigation property, not just the Id
                }
            }

            // Create the employee using the UserManager
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
