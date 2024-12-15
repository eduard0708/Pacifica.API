using Pacifica.API.Dtos.UserManagement;
using Pacifica.API.Helper;
using Pacifica.API.Models.EmployeManagement;

namespace Pacifica.API.Services.EmployeeManagementService
{
    public interface IPositionService
    {
        Task<ApiResponse<IEnumerable<Position>>> GetAllAsync();
        Task<ApiResponse<Position?>> GetByIdAsync(int id);
        Task<ApiResponse<Position>> CreateAsync(PositionDto positionDto);
        Task<ApiResponse<Position?>> UpdateAsync(int id, PositionDto positionDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
