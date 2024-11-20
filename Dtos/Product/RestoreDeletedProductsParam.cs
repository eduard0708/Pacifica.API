namespace Pacifica.API.Dtos.Product
{
    public class RestoreDeletedProductsParam
    {
        public List<int>? ProductIds { get; set; }
        public string? Remarks { get; set; }
        public string? RestoredBy { get; set; }
    }
}