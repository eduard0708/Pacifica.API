namespace Pacifica.API.Dtos.F152Report
{
    public class F152ReportTransactionDto
    {
        public int Id { get; set; }
        public string? TransactionName { get; set; }
        public int BranchId { get; set; }
        public List<F152ReportCategoryDto>? F152ReportCategories { get; set; }
    }
}