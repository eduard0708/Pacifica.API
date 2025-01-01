
using Pacifica.API.Dtos.Branch;

namespace Pacifica.API.Dtos.Inventory
{
    public class BeginningInventoryDto
    {
        public int Id { get; set; }

        public decimal VeterinaryValue { get; set; }
        public string? VeterinaryName { get; set; }
        public string? VeterinaryIcon { get; set; }
        public string? VeterinaryColor { get; set; }

        public decimal SpecialtyFeedsValue { get; set; }
        public string? SpecialtyFeedsName { get; set; }
        public string? SpecialtyFeedsIcon { get; set; }
        public string? SpecialtyFeedsColor { get; set; }

        public decimal AgriculturalFeedsValue { get; set; }
        public string? AgriculturalFeedsName { get; set; }
        public string? AgriculturalFeedsIcon { get; set; }
        public string? AgriculturalFeedsColor { get; set; }

        public decimal DayOldChicksValue { get; set; }
        public string? DayOldChicksName { get; set; }
        public string? DayOldChicksIcon { get; set; }
        public string? DayOldChicksColor { get; set; }

        public decimal ChemicalsOthersValue { get; set; }
        public string? ChemicalsOthersName { get; set; }
        public string? ChemicalsOthersIcon { get; set; }
        public string? ChemicalsOthersColor { get; set; }

        public decimal FertilizersSeedsValue { get; set; }
        public string? FertilizersSeedsName { get; set; }
        public string? FertilizersSeedsIcon { get; set; }
        public string? FertilizersSeedsColor { get; set; }
        public DateTime BeginningInventoryDate { get; set; }
        public int? BranchId { get; set; }  // Nullable if it can be null
        public BranchDto? Branch { get; set; }
    }
}