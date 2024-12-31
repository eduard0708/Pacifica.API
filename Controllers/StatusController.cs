using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Status;
using Pacifica.API.Services.StatusService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IMapper _mapper;

        public StatusController(IStatusService statusService, IMapper mapper)
        {
            _statusService = statusService;
            _mapper = mapper;
        }

        // GET: api/Status/all
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusDto>>>> GetStatuses()
        {
            var response = await _statusService.GetAllStatusesAsync();
            var statusDtos = _mapper.Map<IEnumerable<StatusDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<StatusDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = statusDtos
            });
        }

        // GET: api/Status/select
        // Respond for the select list in the front-end application
        [HttpGet("select")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SelectStatusDto>>>> GetSelectStatuses()
        {
            var response = await _statusService.GetSelectStatusesAsync();
            return Ok(new ApiResponse<IEnumerable<SelectStatusDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        // GET: api/Status
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusDto>>>> GetStatusesByPageAsync(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 5,
            [FromQuery] string sortField = "statusName",  // Default sort field
            [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
        )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<StatusDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<StatusDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<StatusDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "statusName", "createdAt", "isActive" }; // Add more fields as needed

            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<StatusDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get statuses by page, passing sortField and sortOrder
            var response = await _statusService.GetStatusesByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var statusDtos = _mapper.Map<IEnumerable<StatusDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<StatusDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = statusDtos,
                TotalCount = response.TotalCount
            });
        }

        // GET: api/Status/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StatusDto>>> GetStatus(int id)
        {
            var response = await _statusService.GetStatusByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var statusDto = _mapper.Map<StatusDto>(response.Data);
            return Ok(new ApiResponse<StatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = statusDto
            });
        }

        // POST: api/Status
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StatusDto>>> CreateStatus(CreateStatusDto statusDto)
        {
            var status = _mapper.Map<Status>(statusDto);
            var response = await _statusService.CreateStatusAsync(status);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdStatusDto = _mapper.Map<StatusDto>(response.Data);
            return CreatedAtAction("GetStatus", new { id = response.Data!.Id }, new ApiResponse<StatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdStatusDto
            });
        }

        // PUT: api/Status/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto statusDto)
        {
            var status = _mapper.Map<Status>(statusDto);
            var response = await _statusService.UpdateStatusAsync(id, status);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedStatusDto = _mapper.Map<StatusDto>(response.Data);
            return Ok(new ApiResponse<StatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedStatusDto
            });
        }

        // DELETE: api/Status/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var response = await _statusService.DeleteStatusAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
