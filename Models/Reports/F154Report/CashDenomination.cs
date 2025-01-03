using System.ComponentModel.DataAnnotations.Schema;
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class CashDenomination
    {
        public int Id { get; set; } // Unique identifier
        public DenominationEnums? Denomination { get; set; }  // Use enum for regular denominations, nullable for assorted coins
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int F154SalesReportId { get; set; }
        public F154SalesReport? F154SalesReport { get; set; }

    }
}
