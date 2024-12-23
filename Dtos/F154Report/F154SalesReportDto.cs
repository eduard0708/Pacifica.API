namespace Pacifica.API.Dtos.F154Report
{
    public class F154SalesReportDto
    {
        public int Id { get; set; } // Unique identifier for the report
        public DateTime DateReported { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public decimal SalesForTheDay { get; set; }
        public decimal GrossSalesCRM { get; set; }
        public decimal GrossSalesCashSlip { get; set; }
        public decimal OverAllTotal { get; set; }

        public decimal NetAccountability { get; set; }

        public decimal TotalDenominations { get; set; }
        public decimal TotalChecksAmount { get; set; }
        public decimal TotalCashCount { get; set; }
        public decimal CashShortOver { get; set; }

        public decimal PerCapita { get; set; }
        public decimal TotalSalesBreakDown { get; set; }
        public int CustomerCounts { get; set; }

        public decimal CashSlip { get; set; }
        public decimal ChargeInvoice { get; set; }
        public decimal PaymentsOfAccounts { get; set; }
        public decimal OtherReceipts { get; set; }
        public string? CreatedBy { get; set; }
        public string? CertifiedBy { get; set; }
        public string? ApprovedBy { get; set; }
        // Related data
        public List<CashDenominationDto>? CashDenominations { get; set; } 
        public List<SalesBreakdownDto>? SalesBreakDowns { get; set; }
        public List<CheckDto>? Checks { get; set; } 
        public List<InclusiveInvoiceTypeDto>? InclusiveInvoiceTypes { get; set; } 
        public LessDto? less { get; set; }

    }
}