using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models.Reports.F154Report
{
    public class Less
    {
        public int Id { get; set; } // Primary key
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OverPunch { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalesReturnOP { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ChargeSales { get; set; }

        // Foreign Key
        public int F154SalesReportId { get; set; }  // One-to-One relationship with SalesReport
        public F154SalesReport? F154SalesReport { get; set; }

    }
}