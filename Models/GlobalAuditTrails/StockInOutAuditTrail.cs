using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class StockInOutAuditTrail : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        // Navigation properties
        public int StockInOutId { get; set; }

        [JsonIgnore]
        public StockInOut? StockInOut { get; set; }
    }
}