
using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class BranchProductAuditTrail : AuditTrail
    {

        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public BranchProduct? BranchProduct { get; set; }
    }
}