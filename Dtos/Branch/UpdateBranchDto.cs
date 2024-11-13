namespace Pacifica.API.Dtos.Branch
{
    public class UpdateBranchDto
    {
        public string? BranchName { get; set; }
        public string? BranchLocation { get; set; }
        public bool IsActive { get; set; }
    }
}