
using Pacifica.API.Models.Menu;

namespace Pacifica.API.Dtos.Menu
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public int? ParentId { get; set; }  // To handle nested menus

        // Navigation properties
        public MenuDto? Parent { get; set; }  // The parent menu (self-referencing)
        public List<MenuDto> Items { get; set; } = new List<MenuDto>();  // Child menus (submenus)
        public List<UserMenuDto>? UserMenus { get; set; }  // Navigation to UserMenu
    }
}