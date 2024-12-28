using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Menu;
using Pacifica.API.Models.Menu;

namespace Pacifica.API.Services.MenuService
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        // Injecting AutoMapper
        public MenuService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all menus
        public async Task<ApiResponse<List<Menu>>> GetAllMenusAsync()
        {
            var menus = await _context.Menus
                                      .Where(m => m.ParentId == null) // Get top-level menus (without parents)
                                      .Include(m => m.Items) // Include child items (submenus)
                                      .ToListAsync();

            // Map List<Menu> to List<MenuDto> using AutoMapper
            var menuDtos = _mapper.Map<List<Menu>>(menus);

            return new ApiResponse<List<Menu>>
            {
                Success = true,
                Data = menuDtos
            };
        }

        // Get menus assigned to a specific employee
        public async Task<ApiResponse<List<Menu>>> GetMenusByEmployeeIdAsync(string employeeId)
        {
            var userMenus = await _context.UserMenus
                                           .Where(um => um.EmployeeId == employeeId)
                                           .Include(um => um.Menu)
                                           .Select(um => um.Menu)
                                           .ToListAsync();

            // Map List<Menu> to List<MenuDto> using AutoMapper
            var menuDtos = _mapper.Map<List<Menu>>(userMenus);

            return new ApiResponse<List<Menu>>
            {
                Success = true,
                Data = menuDtos
            };
        }

        // Assign menus to an employee
        public async Task<ApiResponse<bool>> AssignMenusToEmployeeAsync(string employeeId, List<int> menuIds)
        {
            var employee = await _context.Users.FindAsync(employeeId);
            if (employee == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Employee not found."
                };
            }

            // Remove existing assignments for this employee
            var existingAssignments = await _context.UserMenus
                                                      .Where(um => um.EmployeeId == employeeId)
                                                      .ToListAsync();
            _context.UserMenus.RemoveRange(existingAssignments);

            // Add new assignments
            var userMenus = menuIds.Select(menuId => new UserMenu
            {
                EmployeeId = employeeId,
                MenuId = menuId
            }).ToList();

            await _context.UserMenus.AddRangeAsync(userMenus);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true
            };
        }

        // Remove menu assignments from an employee
        public async Task<ApiResponse<bool>> RemoveMenuAssignmentsFromEmployeeAsync(string employeeId, List<int> menuIds)
        {
            var userMenusToRemove = await _context.UserMenus
                                                   .Where(um => um.EmployeeId == employeeId && menuIds.Contains(um.MenuId))
                                                   .ToListAsync();

            if (userMenusToRemove.Count == 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "No menu assignments found to remove."
                };
            }

            _context.UserMenus.RemoveRange(userMenusToRemove);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true
            };
     
        }
    
     // Implement CreateMenuAsync to create a new menu
        public async Task<ApiResponse<MenuDto>> CreateMenuAsync(CreateMenuDto createMenuDto)
        {
            // Map CreateMenuDto to Menu model
            var newMenu = _mapper.Map<Menu>(createMenuDto);

            // Add the new menu to the database
            _context.Menus.Add(newMenu);
            await _context.SaveChangesAsync();

            // Map the saved Menu to MenuDto to return the created menu
            var menuDto = _mapper.Map<MenuDto>(newMenu);

            return new ApiResponse<MenuDto>
            {
                Success = true,
                Data = menuDto
            };
        }
    
    }
}
