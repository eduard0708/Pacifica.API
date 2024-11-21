using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.BranchProduct
{
    public class SoftDeleteBranchProductParams
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string? DeletedBy { get; set; }
    }
}