namespace Pacifica.API.Services.StatusService
{
    public interface IStatusService
    {
        Task<ApiResponse<IEnumerable<Status>>> GetAllStatusesAsync();
        Task<ApiResponse<Status>> GetStatusByIdAsync(int id);
        Task<ApiResponse<Status>> CreateStatusAsync(Status Status);
        Task<ApiResponse<Status>> UpdateStatusAsync(int id, Status Status);
        // Task<ApiResponse<bool>> DeleteStatusAsync(int id);
    }
}