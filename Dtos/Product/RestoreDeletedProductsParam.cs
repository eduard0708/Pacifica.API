namespace Pacifica.API.Dtos.Product
{
    public class RestoreDeletedProductsParam
    {
        public List<int>? ProductIds { get; set; }
        public List<string>? EmployeeIds { get; set; }
    }
}