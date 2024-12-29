using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Dtos.Role;

namespace Pacifica.API.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;


        public EmployeeService(UserManager<Employee> userManager, IMapper mapper, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;

        }


        // public async Task<ApiResponse<bool>> CreateEmployeeAsync(CreateEmployeeDto createDto)
        // {
        //     // Manually create the Employee entity from RegisterDto
        //     var employee = new Employee
        //     {
        //         EmployeeId = createDto.EmployeeId,
        //         FirstName = createDto.FirstName,
        //         LastName = createDto.LastName,
        //         Email = createDto.Email,
        //         UserName = createDto.Email, // Using email as username
        //         PositionId = createDto.Position,
        //         DepartmentId = createDto.Department,
        //         IsActive = true, // Set the default value for IsActive
        //         IsDeleted = false, // Set the default value for IsDeleted
        //         CreatedBy = "System" // You can customize this based on who creates the employee
        //     };

        //     // Optionally load related entities to ensure EF Core understands the relationships
        //     if (createDto.Department.HasValue)
        //     {
        //         var department = await _context.Departments.FindAsync(createDto.Department.Value);
        //         if (department != null)
        //         {
        //             employee.Department = department;
        //         }
        //     }

        //     if (createDto.Position.HasValue)
        //     {
        //         var position = await _context.Positions.FindAsync(createDto.Position.Value);
        //         if (position != null)
        //         {
        //             employee.Position = position;
        //         }
        //     }

        //     // Create the employee using the UserManager (for handling Identity)
        //     var result = await _userManager.CreateAsync(employee, createDto.Password!);

        //     // Check if the creation succeeded
        //     if (result.Succeeded)
        //     {
        //         // Now handle roles and branches (assuming you have some logic to map these)

        //         if (createDto.Roles != null && createDto.Roles.Any())
        //         {
        //             // Add roles to the employee
        //             foreach (var role in createDto.Roles)
        //             {
        //                 if (!await _roleManager.RoleExistsAsync(role))
        //                 {
        //                     // Optionally, create the role if it doesn't exist
        //                     var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
        //                     if (!roleResult.Succeeded)
        //                     {
        //                         return new ApiResponse<bool>
        //                         {
        //                             Success = false,
        //                             Message = "Failed to create role.",
        //                             Data = false
        //                         };
        //                     }
        //                 }

        //                 await _userManager.AddToRoleAsync(employee, role);
        //             }
        //         }

        //         if (createDto.Branches != null && createDto.Branches.Any())
        //         {
        //             // Assuming you have a logic to map branches (e.g., adding to a join table)
        //             foreach (var branchId in createDto.Branches)
        //             {
        //                 // Assuming you have a Branch entity that you can fetch by ID
        //                 var branch = await _context.Branches.FindAsync(branchId);
        //                 if (branch != null)
        //                 {
        //                     // Add the branch to the employee through the join table
        //                     employee.EmployeeBranches!.Add(new EmployeeBranch
        //                     {
        //                         EmployeeId = employee.EmployeeId,
        //                         BranchId = branchId
        //                     });
        //                 }
        //             }
        //         }

        //         await _context.SaveChangesAsync();


        //         return new ApiResponse<bool>
        //         {
        //             Success = true,
        //             Message = "Employee created successfully",
        //             Data = true
        //         };
        //     }

        //     // If there were errors, return a failure response with the error messages
        //     return new ApiResponse<bool>
        //     {
        //         Success = false,
        //         Message = "Error creating employee",
        //         Data = false
        //     };
        // }


        public async Task<ApiResponse<bool>> CreateEmployeeAsync(CreateEmployeeDto createDto)
        {
            // Check if createDto is null
            if (createDto == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "CreateEmployeeDto is null.",
                    Data = false
                };
            }

            // Manually create the Employee entity from CreateEmployeeDto
            var employee = new Employee
            {
                EmployeeId = createDto.EmployeeId,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                UserName = createDto.Username, // Using email as username
                PositionId = createDto.Position,
                DepartmentId = createDto.Department,
                IsActive = true, // Set the default value for IsActive
                IsDeleted = false, // Set the default value for IsDeleted
                CreatedBy = "System" // You can customize this based on who creates the employee
            };

            // Initialize EmployeeBranches if it is null
            employee.EmployeeBranches = employee.EmployeeBranches ?? new List<EmployeeBranch>();

            // Optionally load related entities to ensure EF Core understands the relationships
            if (createDto.Department.HasValue)
            {
                var department = await _context.Departments.FindAsync(createDto.Department.Value);
                if (department != null)
                {
                    employee.Department = department;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Department not found.",
                        Data = false
                    };
                }
            }

            if (createDto.Position.HasValue)
            {
                var position = await _context.Positions.FindAsync(createDto.Position.Value);
                if (position != null)
                {
                    employee.Position = position;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Position not found.",
                        Data = false
                    };
                }
            }

            // Create the employee using the UserManager (for handling Identity)
            var result = await _userManager.CreateAsync(employee, createDto.Password!);

            // Check if the creation succeeded
            if (result.Succeeded)
            {
                // Now handle roles and branches (assuming you have some logic to map these)

                if (createDto.Roles != null && createDto.Roles.Any())
                {
                    // Add roles to the employee
                    foreach (var role in createDto.Roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            // Optionally, create the role if it doesn't exist
                            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                            if (!roleResult.Succeeded)
                            {
                                return new ApiResponse<bool>
                                {
                                    Success = false,
                                    Message = "Failed to create role.",
                                    Data = false
                                };
                            }
                        }

                        await _userManager.AddToRoleAsync(employee, role);
                    }
                }

                if (createDto.Branches != null && createDto.Branches.Any())
                {
                    // Assuming you have a logic to map branches (e.g., adding to a join table)
                    foreach (var branchId in createDto.Branches)
                    {
                        // Assuming you have a Branch entity that you can fetch by ID
                        var branch = await _context.Branches.FindAsync(branchId);
                        if (branch != null)
                        {
                            // Add the branch to the employee through the join table
                            employee.EmployeeBranches!.Add(new EmployeeBranch
                            {
                                EmployeeId = employee.EmployeeId,
                                BranchId = branchId,
                                Role = "DefaultRole" // You should replace this with a valid role if needed

                            });
                        }
                        else
                        {
                            // Handle the case when a branch is not found
                            return new ApiResponse<bool>
                            {
                                Success = false,
                                Message = $"Branch with ID {branchId} not found.",
                                Data = false
                            };
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Data = true
                };
            }

            // If there were errors, return a failure response with the error messages
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Error creating employee",
                Data = false
            };
        }


        public async Task<ApiResponse<EmployeeDto>> RegisterEmployeeAsync(RegisterUserDto registerUser)
        {
            // Create a new Employee (Identity User)
            var employee = new Employee
            {
                UserName = registerUser.EmployeeId,
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

            // Map the employee to EmployeeDto using AutoMapper
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new ApiResponse<EmployeeDto>
            {
                Success = true,
                Data = employeeDto
            };
        }

        public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeesAsync()
        {
            // Fetch employees with related data (Department, Position, and EmployeeBranches)
            var employees = await _userManager.Users
                .Include(e => e.Department)             // Assuming Department is an entity linked to Employee
                .Include(e => e.Position)               // Assuming Position is an entity linked to Employee
                .Include(e => e.EmployeeBranches)       // Include EmployeeBranches related data
                .ToListAsync();                         // Execute the query to retrieve the data

            // For each employee, fetch their roles asynchronously, including the role ID
            // For each employee, fetch their roles asynchronously, including the role ID
            foreach (var employee in employees)
            {
                // Get the list of role names assigned to the employee
                var roleNames = await _userManager.GetRolesAsync(employee);

                // Fetch the role objects based on the role names
                var rolesWithIds = new List<RoleDto>(); // Assuming RoleDto is a DTO with properties like Id and Name

                foreach (var roleName in roleNames)
                {
                    // Fetch the role object by name using _roleManager
                    var role = await _roleManager.FindByNameAsync(roleName);

                    if (role != null)
                    {
                        // Add the role ID and Name to the RoleDto list
                        rolesWithIds.Add(new RoleDto
                        {
                            Id = role.Id,    // The Role ID
                            Name = role.Name // The Role Name
                        });
                    }
                }

                // Assign the roles with IDs to the employee object
                employee.Roles = rolesWithIds;
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
                Position = employee.Position?.Name,     // Handle null values in Position

                // Roles are set directly here from the employee entity (with ID included)
                Roles = employee.Roles, // Roles are already assigned to employee from the previous logic

                // Check if EmployeeBranches is not null or empty
                Branches = employee.EmployeeBranches!
                                .Select(eb => new BranchDto
                                {
                                    Id = eb.BranchId,  // Map BranchId from EmployeeBranch

                                    // Fetch BranchName from _context.Branches using BranchId
                                    BranchName = _context.Branches
                                        .Where(b => b.Id == eb.BranchId)
                                        .Select(b => b.BranchName)
                                        .FirstOrDefault() ?? "Unknown"  // Default to "Unknown" if Branch is not found
                                }).ToList()

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

        /// <summary>
        /// Edit Employee asign remove roles, assign remove branches,ssign change department, assign change position
        public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto updateDto)
        {
            // Find the employee by their Id
            var employee = await _userManager.Users
                .Include(e => e.EmployeeBranches)  // Include the EmployeeBranches related entities
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return new ApiResponse<EmployeeDto> { Success = false, Message = "Employee not found" };

            // Perform manual mapping from UpdateEmployeeDto to Employee
            employee.FirstName = updateDto.FirstName ?? employee.FirstName;
            employee.LastName = updateDto.LastName ?? employee.LastName;
            employee.Email = updateDto.Email ?? employee.Email;
            employee.DepartmentId = updateDto.DepartmentId ?? employee.DepartmentId;
            employee.PositionId = updateDto.PositionId ?? employee.PositionId;

            // Fetch existing roles assigned to the employee
            var existingRoles = await _userManager.GetRolesAsync(employee);

            // If roles are provided in the DTO, update the roles
            if (updateDto.Roles != null && updateDto.Roles.Any())
            {
                var rolesToAdd = updateDto.Roles.Except(existingRoles).ToList();
                var rolesToRemove = existingRoles.Except(updateDto.Roles).ToList();

                // Remove roles not in the update DTO
                var removeResult = await _userManager.RemoveFromRolesAsync(employee, rolesToRemove);
                if (!removeResult.Succeeded)
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Error removing roles" };

                // Add new roles from the update DTO
                var addResult = await _userManager.AddToRolesAsync(employee, rolesToAdd);
                if (!addResult.Succeeded)
                    return new ApiResponse<EmployeeDto> { Success = false, Message = "Error adding roles" };
            }

            // Fetch the current branches assigned to the employee
            var existingBranches = employee.EmployeeBranches?.Select(b => b.BranchId).ToList() ?? new List<int>();

            if (updateDto.BranchIds != null && updateDto.BranchIds.Any())
            {
                // Determine which branches to add and remove
                var branchesToAdd = updateDto.BranchIds.Except(existingBranches).ToList();
                var branchesToRemove = existingBranches.Except(updateDto.BranchIds).ToList();

                // Remove branches that should no longer be associated with the employee
                foreach (var branchId in branchesToRemove)
                {
                    var branchToRemove = employee.EmployeeBranches!.FirstOrDefault(b => b.BranchId == branchId);
                    if (branchToRemove != null)
                    {
                        employee.EmployeeBranches!.Remove(branchToRemove);
                    }
                }

                // Ensure EmployeeBranches is initialized if it is null
                if (employee.EmployeeBranches == null)
                {
                    employee.EmployeeBranches = new List<EmployeeBranch>();
                }

                // Get all branches that need to be added
                var branchesToAddEntities = await _context.Branches
                    .Where(b => branchesToAdd.Contains(b.Id))
                    .ToListAsync();

                // Add branches to the employee's EmployeeBranches
                foreach (var branch in branchesToAddEntities)
                {
                    // Check if the branch is already associated with the employee (this is the fix for avoiding duplicates)
                    if (!employee.EmployeeBranches!.Any(eb => eb.BranchId == branch.Id))
                    {
                        var employeeBranch = new EmployeeBranch
                        {
                            EmployeeId = employeeId,
                            BranchId = branch.Id,
                            Role = "DefaultRole"  // Ensure that role is not null
                        };

                        // Add the new EmployeeBranch to the employee's list
                        employee.EmployeeBranches!.Add(employeeBranch);
                    }
                }
            }

            // Attempt to update the employee in the database
            var result = await _userManager.UpdateAsync(employee);
            if (!result.Succeeded)
            {
                return new ApiResponse<EmployeeDto> { Success = false, Message = "Error updating employee" };
            }

            // Save changes to the EmployeeBranches table
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ApiResponse<EmployeeDto> { Success = false, Message = $"Error saving changes: {ex.Message}" };
            }


            // Get the list of role names assigned to the employee
            var roleNames = await _userManager.GetRolesAsync(employee);

            // Fetch the role objects based on the role names
            var rolesWithIds = new List<RoleDto>(); // Assuming RoleDto is a DTO with properties like Id and Name

            foreach (var roleName in roleNames)
            {
                // Fetch the role object by name using _roleManager
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    // Add the role ID and Name to the RoleDto list
                    rolesWithIds.Add(new RoleDto
                    {
                        Id = role.Id,    // The Role ID
                        Name = role.Name // The Role Name
                    });
                }
            }

            // Assign the roles with IDs to the employee object
            employee.Roles = rolesWithIds;



            // Prepare and return the updated EmployeeDto
            var updatedEmployeeDto = new EmployeeDto
            {
                Id = employee.Id,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department?.Name ?? "Unknown",
                Position = employee.Position?.Name ?? "Unknown",
                Roles = rolesWithIds,
                Branches = _context.Branches
                    .Where(b => employee.EmployeeBranches!.Select(eb => eb.BranchId).Contains(b.Id))
                    .Select(b => new BranchDto
                    {
                        Id = b.Id,
                        BranchName = b.BranchName
                    })
                    .ToList()
            };

            return new ApiResponse<EmployeeDto>
            {
                Success = true,
                Data = updatedEmployeeDto
            };
        }

        // Method to check if employeeId or email exists
        public async Task<ApiResponse<bool>> CheckIfExistsAsync(string value, string type)
        {
            var response = new ApiResponse<bool>
            {
                Success = false,  // Default success status
                Message = "Validation failed",  // Default message
                Data = false,  // Default data value (false, meaning not found)
                TotalCount = 0  // Optional field, can be set to 0 if not needed
            };

            try
            {
                // Validate input
                if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(type))
                {
                    response.Message = "Value or type cannot be null or empty.";
                    return response; // Return early if input is invalid
                }

                // Check if type is 'employeeId'
                if (type == "employeeId")
                {
                    var user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.EmployeeId == value);

                    response.Data = user != null;  // Set existence status (true/false)
                    response.Success = true;  // Operation was successful
                    response.Message = response.Data ? "Employee ID exists." : "Employee ID does not exist.";
                }
                // Check if type is 'email'
                else if (type == "email")
                {
                    var user = await _userManager.FindByEmailAsync(value);

                    response.Data = user != null;  // Set existence status (true/false)
                    response.Success = true;  // Operation was successful
                    response.Message = response.Data ? "Email exists." : "Email does not exist.";
                }
                else
                {
                    response.Message = "Invalid type provided.";  // If invalid type
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;  // Return the ApiResponse object
        }


    }
}
