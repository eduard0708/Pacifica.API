using System.ComponentModel.DataAnnotations;
using Pacifica.API.Models.Inventory;
using Pacifica.API.Models.Reports.F154Report;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Models
{
    public class Branch : AuditDetails
    {
        [Key]
        public int Id { get; set; }  // Primary key for the Branch entity

        [Required(ErrorMessage = "Branch name is required.")]
        [MaxLength(255, ErrorMessage = "Branch name cannot exceed 255 characters.")]
        public string BranchName { get; set; } = string.Empty;  // Name of the branch

        [Required(ErrorMessage = "Branch location is required.")]
        [MaxLength(500, ErrorMessage = "Branch location cannot exceed 500 characters.")]
        public string BranchLocation { get; set; } = string.Empty;  // Location of the ranch

        public ICollection<EmployeeBranch>? EmployeeBranches { get; set; }  // Many-to-many relation with Employee

        public ICollection<BranchProduct>? BranchProducts { get; set; }  // Many-to-many relation with Product

        public ICollection<StockOut>? StockOuts { get; set; }  // Many-to-many relation with Product
        public ICollection<StockIn>? StockIns { get; set; }  // Many-to-many relation with Product

        public ICollection<F154SalesReport>? F154SalesReports { get; set; }  // Many-to-many relation with F154SalesReport

        public ICollection<BeginningInventory>? BeginningInventories { get; set; }

    }
}

