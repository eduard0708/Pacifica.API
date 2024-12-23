using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.F154Report
{
    public class InclusiveInvoiceTypeDto
{
        public int Id { get; set; }
        public InclusiveInvoiceTypeEnums InclusiveInvoiceTypes { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public decimal Amount { get; set; }
    }
}