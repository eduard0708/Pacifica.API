using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.F154Report
{
    public class CheckDto
    {
         public int Id { get; set; }
        public string? CheckNumber { get; set; }
        public decimal Amount { get; set; }
    }
}