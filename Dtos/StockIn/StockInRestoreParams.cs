namespace Pacifica.API.Dtos.StockIn
{
    public class StockInRestoreParams
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = "";
        public string ActionBy { get; set; } = "";
    }
}