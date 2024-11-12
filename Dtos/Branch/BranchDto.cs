using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Branch
{
    public class BranchDto
    {
         public int ID { get; set; }
        public string? BranchName { get; set; }
        public string? BranchLocation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}