using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.Employee;

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
                UserName = registerDto.Email,
                PositionId = registerDto.PositionId,
                DepartmentId = registerDto.DepartmentId,
                IsActive = true,  // Set the default value for IsActive
                IsDeleted = false, // Set the default value for IsDeleted
                CreatedBy = "System" // You can customize this, depending on who creates the employee
            };



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
                    // Department = employee.Department?.Name,  // Assuming you want the department name
                    // Position = employee.Position?.Name
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

        // Register a new employee
        public async Task<ApiResponse<EmployeeDto>> RegisterEmployeeAsync(RegisterUserDto registerUser)
        {
            // Create a new Employee (Identity User)
            var employee = new Employee
            {
                UserName = registerUser.EmployeeId,  // Set EmployeeId as UserName
                Email = registerUser.Email,
                EmployeeId = registerUser.EmployeeId,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName
            };

            // Attempt to create the employee with the provided password
            var result = await _userManager.CreateAsync(employee, registerUser.Password!);

            // Check if creation succeeded
            if (result.Succeeded)
            {
                // Manually create the EmployeeDto to return
                var employeeDto = new EmployeeDto
                {
                    Id = employee.Id.ToString(),  // Assuming Id is of type Guid, convert it to string
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                };

                // Return the success response with EmployeeDto data
                return new ApiResponse<EmployeeDto>
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Data = employeeDto
                };
            }

            // If creation failed, return failure response
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
                // .Include(e => e.Roles)   // Include the roles for the employee
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

            // Fetch the roles associated with the employee using UserManager
            var employeeRoles = await _userManager.GetRolesAsync(employee);
            employee.Roles = employeeRoles.ToList();

            // Map the employee to EmployeeDto using AutoMapper
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new ApiResponse<EmployeeDto>
            {
                Success = true,
                Data = employeeDto
            };
        }

        // public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto updateDto)
        // {
        //     // Find the employee by their Id
        //     var employee = await _userManager.FindByIdAsync(employeeId);
        //     if (employee == null) return new ApiResponse<EmployeeDto> { Success = false, Message = "Employee not found" };

        //     // Perform manual mapping from UpdateEmployeeDto to Employee
        //     employee.FirstName = updateDto.FirstName ?? employee.FirstName;  // If null, keep existing value
        //     employee.LastName = updateDto.LastName ?? employee.LastName;
        //     employee.Email = updateDto.Email ?? employee.Email;
        //     employee.PasswordHash = string.IsNullOrEmpty(updateDto.Password) ? employee.PasswordHash : _userManager.PasswordHasher.HashPassword(employee, updateDto.Password); // Hash password if provided
        //     employee.DepartmentId = updateDto.DepartmentId ?? employee.DepartmentId;
        //     employee.PositionId = updateDto.PositionId ?? employee.PositionId;

        //     // Here you would map any other properties you need to update.

        //     // Update employee in the database
        //     var result = await _userManager.UpdateAsync(employee);

        //     // Return the response based on the result of the update operation
        //     if (result.Succeeded)
        //     {
        //         return new ApiResponse<EmployeeDto>
        //         {
        //             Success = true,
        //             Data = new EmployeeDto
        //             {
        //                 EmployeeId = employee.EmployeeId,
        //                 FirstName = employee.FirstName,
        //                 LastName = employee.LastName,
        //                 Email = employee.Email,
        //                 Department = employee.Department!.Name,
        //                 Position = employee.Position!.Name,
        //                 // Map any other fields you need to return in the response
        //             }
        //         };
        //     }

        //     return new ApiResponse<EmployeeDto> { Success = false, Message = "Error updating employee" };
        // }

        public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto updateDto)
        {
            // Find the employee by their Id
            var employee = await _userManager.FindByIdAsync(employeeId);
            if (employee == null) return new ApiResponse<EmployeeDto> { Success = false, Message = "Employee not found" };

            // Perform manual mapping from UpdateEmployeeDto to Employee
            employee.FirstName = updateDto.FirstName ?? employee.FirstName;  // If null, keep existing value
            employee.LastName = updateDto.LastName ?? employee.LastName;
            employee.Email = updateDto.Email ?? employee.Email;
            // employee.PasswordHash = string.IsNullOrEmpty(updateDto.Password) ? employee.PasswordHash : _userManager.PasswordHasher.HashPassword(employee, updateDto.Password); // Hash password if provided
            employee.DepartmentId = updateDto.DepartmentId ?? employee.DepartmentId;
            employee.PositionId = updateDto.PositionId ?? employee.PositionId;

            // Fetch existing roles assigned to the employee
            var existingRoles = await _userManager.GetRolesAsync(employee);

            // If roles are provided in the DTO, update the roles
            if (updateDto.Roles != null && updateDto.Roles.Any())
            {
                // Remove the employee's current roles and assign the new roles
                var rolesToAdd = updateDto.Roles.Except(existingRoles).ToList(); // Roles to add
                var rolesToRemove = existingRoles.Except(updateDto.Roles).ToList(); // Roles to remove

                // Remove the roles from the employee
                var removeResult = await _userManager.RemoveFromRolesAsync(employee, rolesToRemove);

                if (!removeResult.Succeeded)
                {
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Error removing roles" };
                }

                // Add the new roles
                var addResult = await _userManager.AddToRolesAsync(employee, rolesToAdd);

                if (!addResult.Succeeded)
                {
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Error adding roles" };
                }
            }

            // Update employee in the database
            var result = await _userManager.UpdateAsync(employee);

            // Return the response based on the result of the update operation
            if (result.Succeeded)
            {
                return new ApiResponse<EmployeeDto>
                {
                    Success = true,
                    Data = new EmployeeDto
                    {
                        Id = employee.Id,
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        Department = employee.Department?.Name ?? "Unknown",
                        Position = employee.Position?.Name ?? "Unknown",
                        // Explicitly convert IList<string> to List<string>
                        Roles = await _userManager.GetRolesAsync(employee) // Convert the IList to List<string>
                    }
                };
            }

            return new ApiResponse<EmployeeDto> { Success = false, Message = "Error updating employee" };
        }

        public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync()
        {
            // Get all employees with roles, department, position, and branches
            var employees = await _userManager.Users
                .Include(e => e.Department)       // Assuming Department is an entity linked to Employee
                .Include(e => e.Position)         // Assuming Position is an entity linked to Employee
                .Include(e => e.EmployeeBranches) // Include the branches for each employee
                .ToListAsync();

            // For each employee, fetch their roles asynchronously
            foreach (var employee in employees)
            {
                var roles = await _userManager.GetRolesAsync(employee);
                employee.Roles = roles.ToList(); // Manually set the Roles to the employee object
            }

            // Manually map Employee entities to GetEmployeeDto
            var employeeDtos = employees.Select(employee => new GetEmployeeDto
            {
                Id = employee.Id,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department?.Name, // Handle null values in Department
                Position = employee.Position?.Name, // Handle null values in Position
                Roles = employee.Roles, // Roles are set directly here from the employee entity

                // Map Branches to BranchDto list
                Branches = employee.EmployeeBranches?.Select(branch => new BranchDto
                {
                    Id = branch.BranchId,
                    BranchName = branch.Branch?.BranchName // Ensure Branch is not null before accessing
                }).ToList() ?? new List<BranchDto>()  // If EmployeeBranches is null, return an empty list

            }).ToList();

            // Return the response with the mapped employee data
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Success = true,
                Data = employeeDtos
            };
        }

        public async Task<ApiResponse<IEnumerable<GetFilter_Employee>>> GetEmployeesByPageAsync(int page, int pageSize, string sortField, int sortOrder)
        {
            // Map sortField to an actual Expression<Func<Employee, object>> that EF Core can process
            var sortExpression = GetSortExpression(sortField);

            if (sortExpression == null)
            {
                return new ApiResponse<IEnumerable<GetFilter_Employee>>
                {
                    Success = false,
                    Message = "Invalid sort expression.",
                    Data = null,
                    TotalCount = 0
                };
            }

            // Get the total count of employees
            var totalCount = await _context.Users
                .IgnoreQueryFilters() // Ignore QueryFilters for soft delete if necessary
                .CountAsync();

            // Dynamically order the query based on the sort expression and sort order
            IQueryable<Employee> query = _context.Users  // Replace `User` with your actual entity class name
                .IgnoreQueryFilters()  // Ignore global filters, so we can apply soft delete filter manually
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.EmployeeBranches)  // Include EmployeeBranches if needed
                .Select(e => e);  // Start with just the entity

            // Apply sorting dynamically based on sortOrder
            if (sortOrder == 1)
            {
                query = query.OrderBy(sortExpression);  // Apply ascending order
            }
            else
            {
                query = query.OrderByDescending(sortExpression);  // Apply descending order
            }

            // Apply pagination
            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Now retrieve the roles for each employee
            var employeeDtos = new List<GetFilter_Employee>();
            foreach (var employee in employees)
            {
                // Get the roles for each employee using UserManager
                var roles = await _userManager.GetRolesAsync(employee);

                // Create the DTO and map other properties
                var employeeDto = new GetFilter_Employee
                {
                    Id = employee.Id.ToString(),
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DepartmentId = employee.Department?.Id ?? 0,  // Handle null safely
                    Department = employee.Department?.Name,
                    PositionId = employee.Position?.Id ?? 0,  // Handle null safely
                    Position = employee.Position?.Name,
                    Roles = roles.ToList(),  // Store roles as a list of role names
                    Branches = employee.EmployeeBranches?.Select(branch => new BranchDto
                    {
                        Id = branch.BranchId,
                        BranchName = branch.Branch?.BranchName
                    }).ToList() ?? new List<BranchDto>(),  // Handle null safely
                };

                employeeDtos.Add(employeeDto);
            }

            // Return the response with the mapped employee data
            return new ApiResponse<IEnumerable<GetFilter_Employee>>
            {
                Success = true,
                Message = "Employees retrieved successfully.",
                Data = employeeDtos,
                TotalCount = totalCount
            };
        }

        private Expression<Func<Employee, object>> GetSortExpression(string sortField)
        {
            switch (sortField.ToLower())  // Use ToLower() to make it case insensitive
            {
                case "firstname":
                    return e => e.FirstName!;
                case "lastname":
                    return e => e.LastName!;
                case "email":
                    return e => e.Email!;
                case "department":
                    return e => e.Department!.Name!;  // Assuming Department is an entity and has a Name property
                case "position":
                    return e => e.Position!.Name!;  // Assuming Position is an entity and has a Name property
                case "employeeid":
                    return e => e.EmployeeId!;
                default:
                    return null!;  // Invalid sort field
            }
        }

    }

}
