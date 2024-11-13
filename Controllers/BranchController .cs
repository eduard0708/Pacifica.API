using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Services.BranchService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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
        [HttpGet]
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
