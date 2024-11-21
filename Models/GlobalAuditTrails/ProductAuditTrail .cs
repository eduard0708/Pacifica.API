using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class ProductAuditTrail : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        // Navigation properties
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}