using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.Role;
using Pacifica.API.Models.EmployeManagement;

namespace Pacifica.API.Dtos.Admin
{
    public class GetEmployeeDto
    {
        public string? Id { get; set; }  // Change to Guid to match Employee.Id
        public string? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public List<RoleDto>? Roles { get; set; }
        public List<BranchDto>? Branches { get; set; }
    }
}