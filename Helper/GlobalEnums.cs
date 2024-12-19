
namespace Pacifica.API.Helper
{
    public class GlobalEnums
    {

        public enum F512Category
        {
            Veterinary,
            SpecialtyFeeds,
            AgriculturalFeeds,
            DayOldChicks,
            ChemicalsOthers,
            FertilizersSeeds
        }
        public enum ReportPeriod
        {
            Period1_7,
            Period8_15,
            Period16_22,
            Period23_31,
            FifthCutOff
        }


        public enum InventoryStatus
        {
            Pending = 0,
            Completed = 1
        }

        public enum InventoryType
        {
            Weekly = 1,
            Monthly = 2,
            MidYear = 3,
            YearEnd = 4
        }
    }
}