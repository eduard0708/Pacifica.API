using Pacifica.API.Dtos.Admin;
using Pacifica.API.Helper;

namespace Pacifica.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> LoginAsync(LoginDto loginDto);
        // Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto);
    }
}