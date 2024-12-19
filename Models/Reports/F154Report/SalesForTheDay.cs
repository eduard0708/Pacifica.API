using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class SalesForTheDay
    {
        public decimal GrossSalesCRM { get; set; }
        public decimal GrossSalesCashSlip { get; set; }
        public decimal TotalSales { get; set; }
        public decimal OverPunch { get; set; }
        public decimal SalesReturn { get; set; }
        public decimal ChargeSales { get; set; }
        public decimal NetAccountability { get; set; }
    }
}