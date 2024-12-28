using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Menu
{
    public class CreateMenuDto
    {
        public string? Label { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public int? ParentId { get; set; } // For nested submenus
    }
}