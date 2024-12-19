using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class CashAccount
    {
        public List<CashDenomination> Denominations { get; set; }
        public decimal Total { get; set; }
        public decimal ShortOver { get; set; }
    }
}