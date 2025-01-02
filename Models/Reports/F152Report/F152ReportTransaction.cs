namespace Pacifica.API.Models.Reports.F152Report
{
    public class F152ReportTransaction : AuditDetails
    {
        public int Id { get; set; }
        public string? TransactionName { get; set; }
        public int BranchId { get; set; }  // Foreign key
        public Branch? Branch { get; set; }

        // Categories within each section
        public ICollection<F152ReportCategory>? F152ReportCategories { get; set; }
    }
}