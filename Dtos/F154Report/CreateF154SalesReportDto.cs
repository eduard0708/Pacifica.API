using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Dtos.F154Report
{
    public class CreateF154SalesReportDto
    {
    //    public int Id { get; set; }
        public DateTime DateReported { get; set; }
        public int BranchId { get; set; }
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
        public List<CreateCashDenominationDto>? CashDenominations { get; set; } 
        public List<CreateSalesBreakdownDto>? SalesBreakDowns { get; set; }
        public List<CreateCheckDto>? Checks { get; set; } 
        public List<CreateInclusiveInvoiceTypeDto>? InclusiveInvoiceTypes { get; set; } 
        public CreateLessDto? Less { get; set; } 


    }
}