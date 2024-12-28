using Pacifica.API.Dtos.Admin;

namespace Pacifica.API.Dtos.Menu
{
    public class UserMenuDto
    {
        public string? EmployeeId { get; set; }    // Foreign Key to Employee (IdentityUser)
        public int MenuId { get; set; }           // Foreign Key to Menu

        // Navigation properties
        public EmployeeDto? Employee { get; set; }    // Navigation to Employee
        public MenuDto? Menu { get; set; }            // Navigation to Menu
    }
}