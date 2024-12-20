using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.UserManagement;
using Pacifica.API.Models.EmployeManagement;
using Pacifica.API.Services.EmployeeManagementService;


namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        // GET: api/position
        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            var response = await _positionService.GetAllAsync();
            if (!response.Success)
                return BadRequest(response);  // Return a BadRequest if the operation was not successful

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // GET: api/position/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var response = await _positionService.GetByIdAsync(id);
            if (!response.Success)
                return NotFound(response);  // Return 404 if position is not found

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // POST: api/position
        [HttpPost]
        public async Task<IActionResult> CreatePosition([FromBody] PositionDto positionDto)
        {
            if (positionDto == null)
                return BadRequest(new ApiResponse<Position>
                {
                    Success = false,
                    Message = "Invalid position data.",
                    Data = null
                });

            var response = await _positionService.CreateAsync(positionDto);
            return CreatedAtAction(nameof(GetPositionById), new { id = response.Data?.Id }, response);
        }


        // PUT: api/position/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] PositionDto positionDto)
        {
            if (positionDto == null)
                return BadRequest(new ApiResponse<Position>
                {
                    Success = false,
                    Message = "Invalid position data.",
                    Data = null
                });

            var response = await _positionService.UpdateAsync(id, positionDto);
            if (!response.Success)
                return NotFound(response);  // Return 404 if position was not found

            return Ok(response);  // Return 200 OK with the ApiResponse
        }

        // DELETE: api/position/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var response = await _positionService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(response);  // Return 404 if position was not found

            return NoContent();  // Return 204 No Content for successful deletion
        }
    }
}
