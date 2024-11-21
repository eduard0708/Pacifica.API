namespace Pacifica.API.Dtos.BranchProduct
{
    public class RestoreDeletedBranchProductsParams
    {
        public List<int>? BranchIds { get; set; } = new List<int>();
        public List<int>? ProductIds { get; set; } = new List<int>();
        public string? Remarks { get; set; }
        public string? RestoredBy { get; set; }
    }
}