using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class StockInAuditTrail : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StockInId { get; set; } // Foreign key to StockIn

        [JsonIgnore]
        public StockIn? StockIn { get; set; } // Navigation property to StockIn
    }
}