using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Services.BranchService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        // GET: api/Branch
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchDto>>>> GetBranches()
        {
            var response = await _branchService.GetAllBranchesAsync();
            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<BranchDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchDtos
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchDto>>>> GetBranchesByPageAsync(
       [FromQuery] int? page = 1,
       [FromQuery] int? pageSize = 5,
       [FromQuery] string sortField = "branchName",  // Default sort field
       [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
   )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<BranchDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<BranchDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "branchName", "branchLocation", "createdAt", "isDeleted" }; // Add more fields as needed

            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get branches by page, passing sortField and sortOrder
            var response = await _branchService.GetBranchesByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<BranchDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchDtos,
                TotalCount = response.TotalCount
            });
        }

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BranchDto>>> GetBranch(int id)
        {
            var response = await _branchService.GetBranchByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var branchDto = _mapper.Map<BranchDto>(response.Data);
            return Ok(new ApiResponse<BranchDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchDto
            });
        }

        // POST: api/Branch
        [HttpPost]
        public async Task<ActionResult<ApiResponse<BranchDto>>> CreateBranch(CreateBranchDto branchDto)
        {
            var branch = _mapper.Map<Branch>(branchDto);
            var response = await _branchService.CreateBranchAsync(branch);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdBranchDto = _mapper.Map<BranchDto>(response.Data);
            return CreatedAtAction("GetBranch", new { id = response.Data!.Id }, new ApiResponse<BranchDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdBranchDto
            });
        }

        // PUT: api/Branch/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, UpdateBranchDto branchDto)
        {
            var branch = _mapper.Map<Branch>(branchDto);
            var response = await _branchService.UpdateBranchAsync(id, branch);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedBranchDto = _mapper.Map<BranchDto>(response.Data);
            return Ok(new ApiResponse<BranchDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedBranchDto
            });
        }

        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var response = await _branchService.DeleteBranchAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }


    }
}
