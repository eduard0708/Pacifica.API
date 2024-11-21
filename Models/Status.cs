using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class Status :AuditDetails
    {
        public int Id { get; set; }  // Primary key
        public string StatusName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;  // Description of the transaction type

        // Navigation property
        public ICollection<BranchProduct>? BranchProducts { get; set; }  // Related stock transactions

        
    }
}
