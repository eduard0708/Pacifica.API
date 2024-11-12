using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PacificaAPI.Services.RoleService;

namespace PacificaAPI.Controllers
{
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
            var response = await _roleService.GetAllRolesAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateRole([FromBody] string roleName)
        {
            var response = await _roleService.CreateRoleAsync(roleName);
            return Ok(response);
        }

        [HttpPost("assign/{employeeId}/{roleName}")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignRole(string employeeId, string roleName)
        {
            var response = await _roleService.AssignRoleToEmployeeAsync(employeeId, roleName);
            return Ok(response);
        }

        [HttpPost("remove/{employeeId}/{roleName}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveRole(string employeeId, string roleName)
        {
            var response = await _roleService.RemoveRoleFromEmployeeAsync(employeeId, roleName);
            return Ok(response);
        }
    }
}