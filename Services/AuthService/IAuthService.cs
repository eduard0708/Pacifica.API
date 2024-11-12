using PacificaAPI.Dtos.Admin;

namespace PacificaAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto);
    }
}