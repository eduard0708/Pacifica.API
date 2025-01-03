using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Supplier
{
    public class UpdateSupplierDto
    {
        public string? SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}