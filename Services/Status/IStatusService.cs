using Pacifica.API.Dtos.Status;

namespace Pacifica.API.Services.StatusService
{
    public interface IStatusService
    {
        Task<ApiResponse<IEnumerable<Status>>> GetAllStatusesAsync();
        Task<ApiResponse<IEnumerable<SelectStatusDto>>> GetSelectStatusesAsync();
        Task<ApiResponse<IEnumerable<Status>>> GetStatusesByPageAsync(int page, int pageSize, string sortField, int sortOrder);
        Task<ApiResponse<Status>> GetStatusByIdAsync(int id);
        Task<ApiResponse<Status>> CreateStatusAsync(Status status);
        Task<ApiResponse<Status>> UpdateStatusAsync(int id, Status status);
        Task<ApiResponse<bool>> DeleteStatusAsync(int id);
    }
}