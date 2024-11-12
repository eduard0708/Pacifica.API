using System;

namespace Pacifica.API.Models
{
    public class Branch
    {
        public int Branch_ID { get; set; }
        public string Branch_Name { get; set; } = string.Empty;
        public string Branch_Location { get; set; } = string.Empty;
        public int Created_By { get; set; }
        public bool IsActive { get; set; }
    }
}
