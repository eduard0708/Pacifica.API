using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class Check
    {
        public string Maker { get; set; }
        public string Bank { get; set; }
        public string CheckNo { get; set; }
        public decimal Amount { get; set; }
    }
}