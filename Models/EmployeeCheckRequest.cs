using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Models
{
    public class EmployeeCheckRequest
    {
        public string? Value { get; set; }
        public string? Type { get; set; } // "employeeId" or "email"
    }
}