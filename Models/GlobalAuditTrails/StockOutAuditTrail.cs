using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class StockOutAuditTrail : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StockOutId { get; set; } // Foreign key to StockIn

        [JsonIgnore]
        public StockOut? StockOut { get; set; } // Navigation property to StockIn
    }
}