
namespace Pacifica.API.Helper
{
    public class GlobalEnums
    {

        public enum ProductCategoryEnums
        {
            Veterinary = 1,
            SpecialtyFeeds = 2,
            AgriculturalFeeds = 3,
            DayOldChicks = 4,
            ChemicalsOthers =5,
            FertilizersSeeds = 6,
        }
        public enum SalesLess { 
            OverPunch = 1,
            SalesReturnOP = 2,
            ChargeSales= 3,
        }
        public enum ReportPeriod
        {
            Period1_7,
            Period8_15,
            Period16_22,
            Period23_31,
            FifthCutOff
        }

        // Enum for Cash Denominations
        public enum DenominationEnums
        {
            OneThousand = 1000,
            FiveHundred = 500,
            OneHundred = 100,
            Fifty = 50,
            Twenty = 20,
            Ten = 10,
            Five = 5,
            Two = 2,
            One = 1,
            AssortedCoin = 0
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