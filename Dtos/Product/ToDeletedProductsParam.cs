
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.Product
{
    public class ToDeletedProductsParam
    {
        public List<int>? ProductIds { get; set; }

        [Required]
        public string? DeletedBy { get; set; }
    }
}