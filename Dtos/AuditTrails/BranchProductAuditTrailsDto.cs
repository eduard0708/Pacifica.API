namespace Pacifica.API.Dtos.AuditTrails
{
    public class BranchProductAuditTrailsDto
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public string Action { get; set; } = string.Empty; // "Created", "Updated", "Deleted", or "Restored"
        public string? OldValue { get; set; } // JSON or string representation of old data
        public string? NewValue { get; set; } // JSON or string representation of new data
        public string? Remarks { get; set; } // JSON or string representation of new data        
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public string? ActionBy { get; set; } // User who performed the action
    }
}