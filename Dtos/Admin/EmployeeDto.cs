namespace Pacifica.API.Dtos.Admin
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
    }
}