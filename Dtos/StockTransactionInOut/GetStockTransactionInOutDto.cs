using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.StockTransactionInOut
{
    public class GetStockTransactionInOutDto
    {
        public int TransactionId { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int TransactionNumber { get; set; }

        public int TransactionType { get; set; }
        public int StockQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}