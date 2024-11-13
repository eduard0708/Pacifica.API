using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Services.EmployeeService;

namespace Pacifica.API.Controllers
{
   //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployeeById(string id)
        {
            var response = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployeesAsync();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(string id, RegisterDto registerDto)
        {
            var response = await _employeeService.UpdateEmployeeAsync(id, registerDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee(RegisterDto registerDto)
        {
            var response = await _employeeService.CreateEmployeeAsync(registerDto);
            return Ok(response);
        }
    }

}