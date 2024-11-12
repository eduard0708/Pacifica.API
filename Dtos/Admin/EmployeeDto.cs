namespace PacificaAPI.Dtos.Admin
{
    public class EmployeeDto
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; } = true;
    }
}