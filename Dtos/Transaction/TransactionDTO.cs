using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Transaction
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Add more product details as necessary
        public string? Description { get; set; } // Add more product details as necessary
        public decimal Price { get; set; } // Example price
    }
}