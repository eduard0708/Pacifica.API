using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Helper;
using Pacifica.API.Services.AuthService;
using Pacifica.API.Services.EmployeeService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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
        // public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser)
        // {
        //     // Check if the model is valid
        //     if (!ModelState.IsValid)
        //     {
        //         var errorResponse = new ApiResponse<object>
        //         {
        //             Success = false,
        //             Message = "Model validation failed.",
        //             Data = null
        //         };

        //         return BadRequest(errorResponse);
        //     }

        //     // Use the service to register the employee and get the response
        //     var result = await _employeeService.RegisterEmployeeAsync(registerUser);

        //     // Check if the registration succeeded
        //     if (result.Success)
        //     {
        //         return Ok(result); // Return the successful response with employee data
        //     }

        //     // If there were errors, return a failure response
        //     return BadRequest(result); // Return the failure response
        // }
    

        // [HttpPost("create")]
        // public async Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync([FromBody] RegisterDto registerDto)
        // {
        //     // Call the service to handle employee creation
        //     var response = await _employeeService.CreateEmployeeAsync(registerDto);

        //     return response; // Return the response from the service
        // }
   
    }
}