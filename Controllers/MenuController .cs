using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Menu;
using Pacifica.API.Helper;
using Pacifica.API.Models.Menu;
using Pacifica.API.Services;
using Pacifica.API.Services.MenuService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<Menu>>>> GetAllMenus()
        {
            var response = await _menuService.GetAllMenusAsync();
            return Ok(response);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<Menu>>>> GetMenusByEmployeeId(string employeeId)
        {
            var response = await _menuService.GetMenusByEmployeeIdAsync(employeeId);
            return Ok(response);
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignMenusToEmployee([FromBody] AssignMenuRequest request)
        {
            var response = await _menuService.AssignMenusToEmployeeAsync(request.EmployeeId!, request.MenuIds!);
            return Ok(response);
        }

        [HttpPost("remove")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveMenuAssignments([FromBody] AssignMenuRequest request)
        {
            var response = await _menuService.RemoveMenuAssignmentsFromEmployeeAsync(request.EmployeeId!, request.MenuIds!);
            return Ok(response);
        }
    
     // Endpoint to create a new menu
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<MenuDto>>> CreateMenu([FromBody] CreateMenuDto createMenuDto)
        {
            var response = await _menuService.CreateMenuAsync(createMenuDto);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    
    }

    public class AssignMenuRequest
    {
        public string? EmployeeId { get; set; }
        public List<int>? MenuIds { get; set; }
    }
}
