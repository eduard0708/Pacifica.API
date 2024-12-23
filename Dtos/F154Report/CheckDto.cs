namespace Pacifica.API.Dtos.F154Report
{
    public class CheckDto
    {
        public int Id { get; set; }
        public string? Maker { get; set; }
        public string? Bank { get; set; }
        public string? CheckNumber { get; set; }
        public decimal Amount { get; set; }
    }
}