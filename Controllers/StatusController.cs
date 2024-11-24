using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.StatusService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _StatusService;
        private readonly IMapper _mapper;

        public StatusController(IStatusService StatusService, IMapper mapper)
        {
            _StatusService = StatusService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusDto>>>> GetTransactionType()
        {
            var response = await _StatusService.GetAllStatusesAsync();
            var StatusDtos = _mapper.Map<IEnumerable<StatusDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<StatusDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = StatusDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StatusDto>>> GetTransactionType(int id)
        {
            var response = await _StatusService.GetStatusByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var StatusDto = _mapper.Map<StatusDto>(response.Data);
            return Ok(new ApiResponse<StatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = StatusDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StatusDto>>> CreateTransactionReference(CreateTransactionTypeDto transactionTypeDto)
        {
            var transactionType = _mapper.Map<Status>(transactionTypeDto);
            var response = await _StatusService.CreateStatusAsync(transactionType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdStatusDto = _mapper.Map<StatusDto>(response.Data);
            return CreatedAtAction("GetTransactionType", new { id = response.Data!.Id }, new ApiResponse<StatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdStatusDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatusAsync(int id, UpdateTransactionTypeDto transactionTypeDto)
        {
            var Status = _mapper.Map<Status>(transactionTypeDto);
            var response = await _StatusService.UpdateStatusAsync(id, Status);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedStatusDto = _mapper.Map<Status>(response.Data);
            return Ok(new ApiResponse<Status>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedStatusDto
            });
        }

        // // DELETE: api/TransactionReference/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteTransactionReference(int id)
        // {
        //     var response = await _StatusService.DeleteStatusAsync(id);
        //     if (!response.Success)
        //     {
        //         return NotFound(response);
        //     }

        //     return NoContent();
        // }
    }
}
