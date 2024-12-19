namespace Pacifica.API.Dtos.F154Report
{
    public class SalesBreakdownDto
    {
        public int Id { get; set; }  // Unique identifier for the SalesBreakdown
        public int ProductCategory { get; set; }  // Description of the SalesBreakdown (mapped from ProductCategory or other field)
        public decimal Amount { get; set; }  // Amount of the SalesBreakdown
    }
}