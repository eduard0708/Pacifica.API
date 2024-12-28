
namespace Pacifica.API.Models.Menu
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public int? ParentId { get; set; }  // To handle nested menus

        // Navigation properties
        public Menu? Parent { get; set; }  // The parent menu (self-referencing)
        public List<Menu> Items { get; set; } = new List<Menu>();  // Child menus (submenus)

        public List<UserMenu>? UserMenus { get; set; }  // Navigation to UserMenu
    }
}