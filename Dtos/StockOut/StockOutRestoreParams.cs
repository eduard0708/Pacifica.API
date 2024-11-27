namespace Pacifica.API.Dtos.StockOut
{
    public class StockOutRestoreParams
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = "";
        public string ActionBy { get; set; } = "";
    }
}