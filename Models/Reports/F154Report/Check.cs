using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class Check
    {
        public int Id { get; set;}
        public string? Maker { get; set; }
        public string? Bank { get; set; }
        public string? CheckNumber { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int DailySalesReportId { get; set; }
        public DailySalesReport DailySalesReport { get; set; } = new DailySalesReport();
    }
}