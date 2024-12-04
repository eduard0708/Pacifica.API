namespace Pacifica.API.Dtos.Branch
{
    public class UpdateBranchDto
    {
        public int Id { get; set; }
        public string? BranchName { get; set; }
        public string? BranchLocation { get; set; }
        public bool IsDeleted { get; set; }
    }
}