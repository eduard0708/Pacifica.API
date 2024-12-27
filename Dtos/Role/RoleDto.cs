using System.Text.Json.Serialization;

namespace Pacifica.API.Dtos.Role
{
    public class RoleDto
    {
        [JsonPropertyName("roleId")]  // Renaming property for JSON output
        public string? Id { get; set; }
        public string? Name { get; set; } // Role Name
    }
}