using System.ComponentModel.DataAnnotations;
using Pacifica.API.Models.GlobalAuditTrails;
using Pacifica.API.Models.Transaction;

public class StockIn : AuditDetails
{
    [Key]
    public int Id { get; set; }

    public string? ReferenceNumber { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int BranchId { get; set; }  // Nullable foreign key to Branch

    [Required]
    public int StockInReferenceId { get; set; }

    [Required]
    public DateTime DateReceived { get; set; }
    public DateTime DateReported { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Range(18, 2)]
    public decimal CostPrice { get; set; }

    [Range(18, 2)]
    public decimal RetailPrice { get; set; }

    // Navigation properties
    public Product? Product { get; set; }
    public Branch? Branch { get; set; } = new Branch();
    public StockInReference? StockInReference { get; set; }

    public ICollection<StockInAuditTrail>? StockInAuditTrails { get; set; }

}
