
namespace Pacifica.API.Dtos.Product
{
    public class ProductFilterParams
    {
        public string? Category { get; set; } = string.Empty;
        public string? SKU { get; set; }= string.Empty;
        public string? ProductName { get; set; }= string.Empty;
    }
}