using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.UserManagement;
using Pacifica.API.Models.EmployeManagement;
using Pacifica.API.Services.EmployeeManagementService;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/department
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = await _departmentService.GetAllAsync();
            if (!response.Success)
                return BadRequest(response);  // Return BadRequest if operation fails

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // GET: api/department/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var response = await _departmentService.GetByIdAsync(id);
            if (!response.Success)
                return NotFound(response);  // Return 404 if department not found

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // POST: api/department
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(new ApiResponse<Department>
                {
                    Success = false,
                    Message = "Invalid department data.",
                    Data = null
                });

            var response = await _departmentService.CreateAsync(departmentDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = response.Data?.Id }, response);
        }

        // PUT: api/department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(new ApiResponse<Department>
                {
                    Success = false,
                    Message = "Invalid department data.",
                    Data = null
                });

            var response = await _departmentService.UpdateAsync(id, departmentDto);
            if (!response.Success)
                return NotFound(response);  // Return 404 if department was not found

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // DELETE: api/department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var response = await _departmentService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(response);  // Return 404 if department not found

            return NoContent();  // Return 204 No Content for successful deletion
        }
    }
}
