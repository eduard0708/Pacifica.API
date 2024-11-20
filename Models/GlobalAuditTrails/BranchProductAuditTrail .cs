
namespace Pacifica.API.Models.GlobalAuditTrails
{
    public class BranchProductAuditTrail : AuditTrail<BranchProduct>
    {

        // Composite foreign key properties
        public int BranchId { get; set; }
        public int ProductId { get; set; }

        public BranchProduct? BranchProduct { get; set; }
    }
}