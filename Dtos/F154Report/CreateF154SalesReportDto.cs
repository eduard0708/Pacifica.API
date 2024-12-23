using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Dtos.F154Report
{
    public class CreateF154SalesReportDto
    {
        public DateTime dateReported { get; set; }
        public int BranchId { get; set; }
        public decimal SalesForTheDay { get; set; }
        public decimal GrossSalesCRM { get; set; }
        public decimal GrossSalesCashSlip { get; set; }
        public decimal TotalSales { get; set; }
        public decimal NetAccountability { get; set; }
        public decimal CashSlip { get; set; }
        public decimal ChargeInvoice { get; set; }
        public decimal PaymentsOfAccounts { get; set; }
        public decimal OtherReceipts { get; set; }
        public string? CertifiedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? CreatedBy { get; set; }


        // Related data
        public List<CreateCashDenominationDto>? CashDenominations { get; set; } 
        public List<CreateSalesBreakdownDto>? SalesBreakdowns { get; set; }
        public List<CreateCheckDto>? Checks { get; set; } 
        public CreateLessDto? Less { get; set; } 


    }
}