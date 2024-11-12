using System;

namespace Pacifica.API.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }

        // public string Contact_Email { get; set; }
        // public string Contact_Phone { get; set; }
        // public string Address { get; set; }
        // public string City { get; set; }
        // public string Province { get; set; }
        // public string Postal_Code { get; set; }
        // public string Country { get; set; }
        // public string Website { get; set; }
        // public string Payment_Terms { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created_On { get; set; }
        public DateTime Last_Updated { get; set; }
    }
}
