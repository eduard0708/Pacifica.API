namespace Pacifica.API.Dtos.Admin
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }  // Change to Guid to match Employee.Id
        public int EmployeeId { get; set; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? Department { get; set; }
        public string? Positions { get; set; }
    }
}