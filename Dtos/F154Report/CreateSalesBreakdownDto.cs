using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.F154Report
{
    public class CreateSalesBreakdownDto
    {
        public ProductCategoryEnums ProductCategory { get; set; }  // Description of the SalesBreakdown (mapped from ProductCategory or other field)
        public decimal Amount { get; set; }  // Amount of the SalesBreakdown
    }
}