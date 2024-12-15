
namespace Pacifica.API.Models.EmployeManagement
{
    public class Department:AuditDetails
    {
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Employee>? Employees { get; set; }

    }
}