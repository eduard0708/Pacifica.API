using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Supplier :AuditDetails
    {
        public int Id { get; set; }  // Unique identifier for the supplier

        public string? SupplierName { get; set; }  // Name of the supplier

        public string? ContactPerson { get; set; }  // Name of the contact person at the supplier

        public string? ContactNumber { get; set; }  // Contact number for the supplier
        public ICollection<Product>? Products { get; set; }

    }
}