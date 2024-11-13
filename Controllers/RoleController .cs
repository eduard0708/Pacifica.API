using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Helper;
using Pacifica.API.Services.RoleService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]

    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetAllRoles()
        {
            try
            {
                var response = await _roleService.GetAllRolesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<string>> { Success = false, Message = $"Error retrieving roles: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateRole([FromBody] string roleName)
        {
            try
            {
                var response = await _roleService.CreateRoleAsync(roleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = $"Error creating role: {ex.Message}" });
            }
        }

        [HttpPost("assign/{employeeId}/{roleName}")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignRole(string employeeId, string roleName)
        {
            try
            {
                var response = await _roleService.AssignRoleToEmployeeAsync(employeeId, roleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool> { Success = false, Message = $"Error assigning role: {ex.Message}" });
            }
        }

        [HttpPost("remove/{Id}/{roleName}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveRole(string Id, string roleName)
        {
            try
            {
                var response = await _roleService.RemoveRoleFromEmployeeAsync(Id, roleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool> { Success = false, Message = $"Error removing role: {ex.Message}" });
            }
        }
    }
}
