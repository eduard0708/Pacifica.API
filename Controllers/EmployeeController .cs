using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Services.EmployeeService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
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
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(string id, UpdateEmployeeDto updateDto)
        {
            var response = await _employeeService.UpdateEmployeeAsync(id, updateDto);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<bool>>> CreateEmployee(CreateEmployeeDto createEmpoyee)
        {
            var response = await _employeeService.CreateEmployeeAsync(createEmpoyee);
            return Ok(response);
        }

        // GET: api/Employee
        [HttpGet("page")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetFilter_Employee>>>> GetEmployeesByPageAsync(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 5,
            [FromQuery] string sortField = "department",  // Default sort field
            [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
        )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<GetFilter_Employee>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<GetFilter_Employee>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<GetFilter_Employee>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields for employees
            var validSortFields = new List<string> { "employeeid", "firstname", "lastname", "email", "department", "position" }; // Add more fields as needed


            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<GetFilter_Employee>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get employees by page, passing sortField and sortOrder
            var response = await _employeeService.GetEmployeesByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map the data to EmployeeDto
            var employeeDtos = _mapper.Map<IEnumerable<GetFilter_Employee>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<GetFilter_Employee>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = employeeDtos,
                TotalCount = response.TotalCount
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto createUser)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Success = false,
                    Message = "Model validation failed.",
                    Data = null
                };

                return BadRequest(errorResponse);
            }

            // Use the service to register the employee and get the response
            var result = await _employeeService.RegisterEmployeeAsync(createUser);

            // Check if the registration succeeded
            if (result.Success)
            {
                return Ok(result); // Return the successful response with employee data
            }

            // If there were errors, return a failure response
            return BadRequest(result); // Return the failure response
        }

        // POST method to check if an EmployeeId or Email exists
        // Method to check if employeeId or email exists
        [HttpPost("check")]
        public async Task<IActionResult> CheckIfExists([FromBody] EmployeeCheckRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Value) || string.IsNullOrEmpty(request.Type))
            {
                return BadRequest("Invalid request.");
            }

            // Call the service method to check for existence
            var response = await _employeeService.CheckIfExistsAsync(request.Value, request.Type);

            // Return the result as a JSON object
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }
    }
}

