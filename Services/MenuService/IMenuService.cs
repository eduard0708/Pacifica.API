using Pacifica.API.Dtos.Menu;
using Pacifica.API.Models.Menu;

namespace Pacifica.API.Services.MenuService
{
    public interface IMenuService
    {

        Task<ApiResponse<List<Menu>>> GetAllMenusAsync();

        // Method to get menus assigned to a specific employee
        Task<ApiResponse<List<Menu>>> GetMenusByEmployeeIdAsync(string userId);

        // Method to assign menus to an employee
        Task<ApiResponse<bool>> AssignMenusToEmployeeAsync(string userId, List<int> menuIds);

        // Method to remove menu assignments from an employee
        Task<ApiResponse<bool>> RemoveMenuAssignmentsFromEmployeeAsync(string employeeId, List<int> menuIds);

        // Add new method to create a menu
        Task<ApiResponse<MenuDto>> CreateMenuAsync(CreateMenuDto createMenuDto);
    }
}