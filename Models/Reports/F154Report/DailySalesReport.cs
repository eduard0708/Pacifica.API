using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class DailySalesReport : AuditDetails
    {
        public int Id { get; set; } // Unique identifier for the report
        public DateTime Date { get; set; }
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
        public decimal TotalSales { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LessOverPunch { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LessSalesReturn { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LessChargeSales { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal NetAccountability { get; set; }

        /// <summary>
        /// B. CASH ACCOUNT
        /// </summary>
        public List<CashDenomination>? CashDenominations { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DenominationSumAmount { get; set; }

        /// <summary>
        /// C. SALES
        /// </summary>
        public List<SalesBreakdown>? SalesBreakdowns { get; set; }

        public int CustomerCount { get; set; }

        /// <summary>
        /// LIST OF CHECKS
        /// </summary>
        public List<Check>? Checks { get; set; }

        /// <summary>
        /// LIST OF CHARGE SALES AND CASH SALES
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CashSlip { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ChargeInvoice { get; set; }

        /// <summary>
        /// LIST OF OFFICIAL RECEIPT
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaymentsOfAccounts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OtherReceipts { get; set; }

        public string? CertifiedBy { get; set; }
        public string? ApprovedBy { get; set; }
    }
}
