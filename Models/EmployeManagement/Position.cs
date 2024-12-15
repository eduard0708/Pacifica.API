namespace Pacifica.API.Models.EmployeManagement
{
    public class Position:AuditDetails
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}