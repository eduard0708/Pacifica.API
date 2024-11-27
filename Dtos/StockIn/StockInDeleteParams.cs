namespace Pacifica.API.Dtos.StockIn
{
    public class StockInDeleteParams
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = "";
        public string ActionBy { get; set; } = "";
    }
}