using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class CashDenomination
    {
        public string Denomination { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}