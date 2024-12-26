using Pacifica.API.Dtos.Branch;

namespace Pacifica.API.Dtos.Admin
{
    public class EmployeeDto
    {
        public string? Id { get; set; }  // Change to Guid to match Employee.Id
        public string? EmployeeId { get; set; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
        public List<BranchDto>? Branches { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
    }
}