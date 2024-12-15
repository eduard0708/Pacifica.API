namespace Pacifica.API.Dtos.Admin
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }  // Change to Guid to match Employee.Id
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public List<string>? Department { get; set; }
        public List<string>? Positions { get; set; }
    }
}