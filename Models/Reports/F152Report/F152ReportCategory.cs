using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Reports.F152Report
{
    public class F152ReportCategory :AuditDetails
    {
        public int Id { get; set; }

        public string? Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostWeek1 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailWeek1 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostWeek2 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailWeek2 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostWeek3 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailWeek3 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostWeek4 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailWeek4 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentRetail { get; set; }

        public int F152ReportTransactionId { get; set; }  // Foreign key
        public F152ReportTransaction? F152ReportTransaction { get; set; }
    }
}
