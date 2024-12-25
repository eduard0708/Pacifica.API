namespace Pacifica.API.Dtos.Admin
{
    public class UpdateEmployeeDto
    {
        public string Id { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
