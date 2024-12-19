using System.ComponentModel.DataAnnotations.Schema;
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class SalesBreakdown
    {
        public int Id { get; set; }
        public ProductCategoryEnums ProductCategory { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int DailySalesReportId { get; set; }
        public DailySalesReport DailySalesReport { get; set; } = new DailySalesReport();

    }
}