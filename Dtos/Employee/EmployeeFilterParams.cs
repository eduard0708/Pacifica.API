namespace Pacifica.API.Dtos.Employee
{
    public class EmployeeFilterParams
    {
        public string? EmployeeName { get; set; } = string.Empty;  // Filter by employee name
        public string? Department { get; set; } = string.Empty;    // Filter by department name
        public string? Role { get; set; } = string.Empty;          // Filter by employee role
        public bool? IsActive { get; set; } = null;
    }
}