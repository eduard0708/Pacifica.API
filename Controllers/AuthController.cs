using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Helper;
using Pacifica.API.Services.AuthService;
using Pacifica.API.Services.EmployeeService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;
        public AuthController(IAuthService authService, IEmployeeService employeeService)
        {
            _authService = authService;
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }

        // [HttpPost("register")]
        // public async Task<ActionResult<ApiResponse<string>>> Register(RegisterDto registerDto)
        // {
        //     var response = await _authService.RegisterAsync(registerDto);
        //     return Ok(response);

            
        // }

        [HttpPost("register")]
        public async Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync([FromBody] RegisterDto registerDto)
        {
            // Call the service to handle employee creation
            var response = await _employeeService.CreateEmployeeAsync(registerDto);

            return response; // Return the response from the service
        }
    }
}