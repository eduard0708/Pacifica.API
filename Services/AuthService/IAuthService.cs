using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;

namespace Pacifica.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> LoginAsync(LoginDto loginDto);
        
    }


}