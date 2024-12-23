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
        public decimal LessOverPunch { get; set; }
        public decimal LessSalesReturn { get; set; }
        public decimal LessChargeSales { get; set; }
        public decimal NetAccountability { get; set; }
        public decimal CashSlip { get; set; }
        public decimal ChargeInvoice { get; set; }
        public decimal PaymentsOfAccounts { get; set; }
        public decimal OtherReceipts { get; set; }
        public string? CertifiedBy { get; set; }
        public string? ApprovedBy { get; set; }

        // Related data
        public List<CashDenominationDto> CashDenominations { get; set; } = new List<CashDenominationDto>();
        public List<SalesBreakdownDto> SalesBreakdowns { get; set; } = new List<SalesBreakdownDto>();
        public List<CheckDto> Checks { get; set; } = new List<CheckDto>();
        public List<Less> Lesses { get; set; } = new List<Less>();


    }
}