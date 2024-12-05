
namespace Pacifica.API.Dtos.Supplier
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}