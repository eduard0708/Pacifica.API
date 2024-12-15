using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Pacifica.API.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Employee> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<Employee> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Get all roles
        public async Task<ApiResponse<List<string>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return new ApiResponse<List<string>> { Success = true, Data = roles! };
        }

        // Create a new role
        public async Task<ApiResponse<string>> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return new ApiResponse<string> { Success = true, Message = "Role created successfully", Data = roleName };
            }

            return new ApiResponse<string> { Success = false, Message = "Error creating role" };
        }

        // Assign role to an employee
        public async Task<ApiResponse<bool>> AssignRoleToEmployeeAsync(string employeeId, string roleName)
        {
            var employee = await _userManager.FindByIdAsync(employeeId);
            if (employee == null) return new ApiResponse<bool> { Success = false, Message = "Employee not found" };

            var result = await _userManager.AddToRoleAsync(employee, roleName);

            return new ApiResponse<bool> { Success = result.Succeeded, Message = result.Succeeded ? "Role assigned successfully" : "Error assigning role" };
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
