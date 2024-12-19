using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.F154Report
{
    public class CashDenominationDto
    {
        public int Id { get; set; }
        public DenominationEnums CashDenomination { get; set; }  // Assuming this is an int (value of the denomination)
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}