using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Helper;
using Pacifica.API.Services.AuthService;
using Pacifica.API.Services.EmployeeService;

namespace Pacifica.API.Controllers
{
   // [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }
   
    }
}