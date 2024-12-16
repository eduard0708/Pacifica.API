using Pacifica.API.Dtos.User;

namespace Pacifica.API.Services.RoleService
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleDto>>> GetAllRolesAsync();
        Task<ApiResponse<string>> CreateRoleAsync(string roleName);
        Task<ApiResponse<bool>> AssignRoleToEmployeeAsync(string employeeId, string roleName);
        Task<ApiResponse<bool>> RemoveRoleFromEmployeeAsync(string employeeId, string roleName);
        Task<ApiResponse<string>> EditRoleAsync(string oldRoleName, string newRoleName);  // Add the edit method
    }
}
