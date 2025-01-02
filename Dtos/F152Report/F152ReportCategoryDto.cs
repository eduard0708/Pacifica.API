using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.F152Report
{
    public class F152ReportCategoryDto
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public decimal CostWeek1 { get; set; }
        public decimal RetailWeek1 { get; set; }
        public decimal CostWeek2 { get; set; }
        public decimal RetailWeek2 { get; set; }
        public decimal CostWeek3 { get; set; }
        public decimal RetailWeek3 { get; set; }
        public decimal CostWeek4 { get; set; }
        public decimal RetailWeek4 { get; set; }
        public decimal AdjustmentCost { get; set; }
        public decimal AdjustmentRetail { get; set; }
    }
}