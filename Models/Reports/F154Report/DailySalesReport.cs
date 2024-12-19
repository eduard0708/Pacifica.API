namespace Pacifica.API.Models.Reports.F154Report
{
    public class DailySalesReport
    {
        public int Id { get; set; } // Unique identifier for the report
        public int BranchId { get; set; }
        public DateTime Date { get; set; }
        
        public int SalesForTheDayId { get; set; }  // Foreign key to SalesForTheDay
        public SalesForTheDay SalesForTheDay { get; set; }
        public int CashAccountId { get; set; }    // Foreign key to CashAccount
        public CashAccount CashAccount { get; set; }
        public List<int> SalesBreakdownIds { get; set; }  // Foreign key references for SalesBreakdowns
        public List<SalesBreakdown> SalesBreakdowns { get; set; }
        public List<int> CheckIds { get; set; }  // Foreign key references for Checks
        public List<Check> Checks { get; set; }
        public List<int> SalesTransactionIds { get; set; }  // Foreign key references for SalesTransactions
        public List<SalesTransaction> SalesTransactions { get; set; }
        public List<int> OfficialReceiptIds { get; set; }  // Foreign key references for OfficialReceipts
        public List<OfficialReceipt> OfficialReceipts { get; set; }
        public string CertifiedBy { get; set; }
        public string ApprovedBy { get; set; }
    }
}