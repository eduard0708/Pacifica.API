using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Role;

namespace Pacifica.API.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Employee> _userManager;
        private readonly ILogger<RoleService> _logger;  // Declare the logger


        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<Employee> userManager, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;  // Assign the injected logger

        }

        // Get all roles
        // Define a DTO or anonymous object to hold the role's id and name
        public async Task<ApiResponse<List<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToListAsync();

            return new ApiResponse<List<RoleDto>> { Success = true, Data = roles! };
        }

        // Create a new role
        public async Task<ApiResponse<string>> CreateRoleAsync(RoleDto roleName)
        {
            var role = new IdentityRole(roleName.Name!);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return new ApiResponse<string> { Success = true, Message = "Role created successfully", Data = roleName.Name };
            }

            return new ApiResponse<string> { Success = false, Message = "Error creating role" };
        }

        // Assign role to an employee
        public async Task<ApiResponse<bool>> AssignRolesToEmployeeAsync(string employeeId, List<AssignRoleDto> roles)
        {
            try
            {
                // Log the incoming employeeId to verify it's correct
                _logger.LogInformation($"Assigning roles to EmployeeId: {employeeId}");

                var employee = await _userManager.Users
                                        .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Employee not found" };
                }

                // Log the employee data for debugging
                _logger.LogInformation($"Found Employee: {employee.Id}, UserName: {employee.UserName}");

                foreach (var role in roles)
                {
                    var result = await _userManager.AddToRoleAsync(employee, role.Name!);
                    if (!result.Succeeded)
                    {
                        _logger.LogError($"Error assigning role {role.Name} to employee {employeeId}");
                        return new ApiResponse<bool> { Success = false, Message = $"Error assigning role: {role.Name}", Data = true };
                    }
                }

                return new ApiResponse<bool> { Success = true, Message = "Roles assigned successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning roles: {ex.Message}");
                return new ApiResponse<bool> { Success = false, Message = $"Error assigning roles: {ex.Message}" };
            }
        }

        // Remove role from an employee
        public async Task<ApiResponse<bool>> RemoveRoleFromEmployeeAsync(string employeeId, string roleName)
        {
            var employee = await _userManager.FindByIdAsync(employeeId);
            if (employee == null) return new ApiResponse<bool> { Success = false, Message = "Employee not found" };

            var result = await _userManager.RemoveFromRoleAsync(employee, roleName);

            return new ApiResponse<bool> { Success = result.Succeeded, Message = result.Succeeded ? "Role removed successfully" : "Error removing role" };
        }

        // Edit an existing role's name
        public async Task<ApiResponse<string>> EditRoleAsync(string oldRoleName, string newRoleName)
        {
            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
            {
                return new ApiResponse<string> { Success = false, Message = "Role not found" };
            }

            role.Name = newRoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse<string> { Success = true, Message = "Role updated successfully", Data = newRoleName };
            }

            return new ApiResponse<string> { Success = false, Message = "Error updating role" };
        }
    }
}
