using Pacifica.API.Dtos.Role;

namespace Pacifica.API.Services.RoleService
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleDto>>> GetAllRolesAsync();
        Task<ApiResponse<string>> CreateRoleAsync(RoleDto roleName);
        Task<ApiResponse<bool>> AssignRolesToEmployeeAsync(string Id, List<AssignRoleDto> roles);
        Task<ApiResponse<bool>> RemoveRoleFromEmployeeAsync(string employeeId, string roleName);
        Task<ApiResponse<string>> EditRoleAsync(string oldRoleName, string newRoleName);  // Add the edit method
    }
}
