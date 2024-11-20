
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Product
{
    public class DeletedProductsParam
    {
        public List<int>? ProductIds { get; set; }
        public string? DeletedBy { get; set; }
    }
}