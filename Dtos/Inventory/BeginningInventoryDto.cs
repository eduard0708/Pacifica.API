
namespace Pacifica.API.Dtos.Inventory
{
    public class BeginningInventoryDto
    {
        public int Id { get; set; }
        public decimal VeterinaryValue { get; set; }
        public decimal SpecialtyFeedsValue { get; set; }
        public decimal AgriculturalFeedsValue { get; set; }
        public decimal DayOldChicksValue { get; set; }
        public decimal ChemicalsOthersValue { get; set; }
        public decimal FertilizersSeedsValue { get; set; }
        public DateTime BeginningInventoryDate { get; set; }
        public int BranchId { get; set; }
    }
}