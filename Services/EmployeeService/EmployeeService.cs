using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.Role;

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
            // Manually create the Employee entity from RegisterDto
            var employee = new Employee
            {
                EmployeeId = registerDto.EmployeeId,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,  // Assuming you want to use the email as the username for Identity
                IsActive = true,  // Set the default value for IsActive
                IsDeleted = false, // Set the default value for IsDeleted
                CreatedAt = DateTime.UtcNow, // Set the current time as CreatedAt
                CreatedBy = "System" // You can customize this, depending on who creates the employee
            };

            // Handle Department relationship
            if (registerDto.DepartmentId != null)
            {
                var department = await _context.Departments.FindAsync(registerDto.DepartmentId);
                if (department != null)
                {
                    employee.Department = department;  // Set the navigation property, not just the Id
                }
                else
                {
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Department not found" };
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
                else
                {
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Position not found" };
                }
            }

            // Create the employee using the UserManager (for handling Identity)
            var result = await _userManager.CreateAsync(employee, registerDto.Password!);

            // Check if the creation succeeded
            if (result.Succeeded)
            {
                // Manually create the EmployeeDto to return
                var employeeDto = new EmployeeDto
                {
                    Id = employee.Id,  // Assuming Id is of type Guid, convert it to string
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Department = employee.Department?.Name,  // Assuming you want the department name
                    Positions = employee.Position?.Name
                };

                return new ApiResponse<EmployeeDto>
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Data = employeeDto
                };
            }

            // If there were errors, return a failure response with the error messages
            return new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = "Error creating employee",
                Data = null
            };
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

        // public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync()
        // {
        //     // Get all employees with roles, department, and position
        //     var employees = await _userManager.Users
        //         .Include(e => e.Roles) // Include the roles for each employee
        //         .Include(e => e.Department) // Assuming Department is an entity linked to Employee
        //         .Include(e => e.Position) // Assuming Position is an entity linked to Employee
        //         .ToListAsync();

        //     // Manually map Employee entities to GetEmployeeDto
        //     var employeeDtos = employees.Select(employee => new GetEmployeeDto
        //     {
        //         Id = employee.Id,
        //         FirstName = employee.FirstName,
        //         LastName = employee.LastName,
        //         Email = employee.Email,
        //         Department = employee.Department?.Name,  // Assuming Department is an entity with a Name property
        //         Position = employee.Position?.Name,  // Assuming Position is an entity with a Title property
        //                                               // Mapping Roles to a list of RoleDto objects
        //         Roles = employee.Roles?.Select(role => new RoleDto
        //         {
        //             Id = role.Id.ToString(),  // Assuming role Id is a Guid or similar
        //             RolRole = role.Name  // Assuming role has a Name property
        //         }).ToList() ?? new List<RoleDto>()  // If Roles is null, return an empty list
        //     }).ToList();

        //     // Return the response with the mapped employee data
        //     return new ApiResponse<List<GetEmployeeDto>>
        //     {
        //         Success = true,
        //         Data = employeeDtos
        //     };
        // }

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

        public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync()
        {
            // Get all employees with roles, department, position, and branches
            var employees = await _userManager.Users
                .Include(e => e.Roles) // Include the roles for each employee
                .Include(e => e.Department) // Assuming Department is an entity linked to Employee
                .Include(e => e.Position) // Assuming Position is an entity linked to Employee
                .Include(e => e.EmployeeBranches) // Include the branches for each employee
                .ToListAsync();

            // Manually map Employee entities to GetEmployeeDto
            var employeeDtos = employees.Select(employee => new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department?.Name, 
                Position = employee.Position?.Name,
                                                      
                Roles = employee.Roles?.Select(role => new RoleDto
                {
                    Id = role.Id,
                    RoleName = role.Name
                }).ToList() ?? new List<RoleDto>(),  // If Roles is null, return an empty list
                                                     // Map Branches to BranchDto list
                Branches = employee.EmployeeBranches?.Select(branch => new BranchDto
                {
                    Id = branch.BranchId,
                    BranchName = branch.Branch!.BranchName
                }).ToList() ?? new List<BranchDto>()  // If Branches is null, return an empty list
            }).ToList();

            // Return the response with the mapped employee data
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Success = true,
                Data = employeeDtos
            };
        }

    }

}
