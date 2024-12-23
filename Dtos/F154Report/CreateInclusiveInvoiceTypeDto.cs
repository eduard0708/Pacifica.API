using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.F154Report
{
    public class CreateInclusiveInvoiceTypeDto
{
        public InclusiveInvoiceTypeEnums InclusiveInvoiceTypes { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public decimal Amount { get; set; }
    }
}