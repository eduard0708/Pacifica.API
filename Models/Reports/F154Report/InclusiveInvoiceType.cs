using System.ComponentModel.DataAnnotations.Schema;
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class InclusiveInvoiceType
    {
        public int Id { get; set; }
        public InclusiveInvoiceTypeEnums InclusiveInvoiceTypes { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int F154SalesReportId { get; set; }
        public F154SalesReport? F154SalesReport { get; set; }
    }
}