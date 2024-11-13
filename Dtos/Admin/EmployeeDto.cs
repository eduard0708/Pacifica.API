namespace Pacifica.API.Dtos.Admin
{
    public class EmployeeDto
    {
        public string Id { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; } = true;
    }
}