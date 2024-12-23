using System.ComponentModel.DataAnnotations.Schema;
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class F154SalesReport : AuditDetails
    {
        public int Id { get; set; } // Unique identifier for the report
        public DateTime DateReported { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        /// <summary>
        /// A. SALES FOR THE DAY
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalesForTheDay { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GrossSalesCRM { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GrossSalesCashSlip { get; set; }

        [Column(TypeName = "decimal(18, 2)")]

        public decimal OverAllTotal { get; set; }
       
        public Less? Less { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal NetAccountability { get; set; }

        /// <summary>
        /// B. CASH ACCOUNT
        /// </summary>
        public List<CashDenomination>? CashDenominations { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalDenominations { get; set; }

        //ADD TOTAL CHECKS
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalChecksAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCashCount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CashShortOver { get; set; }


        /// <summary>
        /// C. SALES
        /// </summary>
        public List<SalesBreakdown>? SalesBreakDowns { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PerCapita { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalSalesBreakDown { get; set; }
        public int CustomerCounts { get; set; }

        /// <summary>
        /// LIST OF CHECKS
        /// </summary>
        public List<Check>? Checks { get; set; }

        /// <summary>
        /// LIST OF CHARGE SALES AND CASH SALES
        /// </summary>
        public List<InclusiveInvoiceType>? InclusiveInvoiceTypes { get; set; }
        /// <summary>
        /// LIST OF OFFICIAL RECEIPT
        /// </summary>
        
        public string? CertifiedBy { get; set; }
        public string? ApprovedBy { get; set; }
    }
}
