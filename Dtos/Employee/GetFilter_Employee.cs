using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.Role;

namespace Pacifica.API.Dtos.Employee
{
    public class GetFilter_Employee
    {
        public string? Id { get; set; }  // Change to Guid to match Employee.Id
        public string? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int DepartmentId { get; set; }
        public string? Department { get; set; }
        public int PositionId { get; set; }
        public string? Position { get; set; }
        public List<string>? Roles { get; set; }

        public List<BranchDto>? Branches { get; set; }
    }
}