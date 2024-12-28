namespace Pacifica.API.Models.Menu
{
    public class UserMenu:AuditDetails
    {
        public string? EmployeeId { get; set; }    // Foreign Key to Employee (IdentityUser)
        public int MenuId { get; set; }           // Foreign Key to Menu

        // Navigation properties
        public Employee? Employee { get; set; }    // Navigation to Employee
        public Menu? Menu { get; set; }            // Navigation to Menu
    }
}