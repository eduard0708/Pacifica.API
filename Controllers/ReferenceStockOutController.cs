using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.ReferenceStockOut;
using Pacifica.API.Services.ReferenceStockOutService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceStockOutController : ControllerBase
    {
        private readonly IReferenceStockOutService _referenceStockOutService;
        private readonly IMapper _mapper;

        public ReferenceStockOutController(IReferenceStockOutService referenceStockOutService, IMapper mapper)
        {
            _referenceStockOutService = referenceStockOutService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReferenceStockOutDto>>>> GetReferenceStockOut()
        {
            var response = await _referenceStockOutService.GetAllReferencesStockOutAsync();
            var ReferenceStockOutDtos = _mapper.Map<IEnumerable<ReferenceStockOutDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<ReferenceStockOutDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = ReferenceStockOutDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReferenceStockOutDto>>> GetTransactionReference(int id)
        {
            var response = await _referenceStockOutService.GetReferenceStockOutByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var ReferenceStockOutDto = _mapper.Map<ReferenceStockOutDto>(response.Data);
            return Ok(new ApiResponse<ReferenceStockOutDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = ReferenceStockOutDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReferenceStockOutDto>>> CreateTransactionReference(CreateReferenceStockOutDto referenceStockOutDto)
        {
            var transactionReference = _mapper.Map<ReferenceStockOut>(referenceStockOutDto);
            var response = await _referenceStockOutService.CreateReferenceStockOutAsync(transactionReference);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdReferenceStockOutDto = _mapper.Map<ReferenceStockOutDto>(response.Data);
            return CreatedAtAction("GetTransactionReference", new { id = response.Data!.Id }, new ApiResponse<ReferenceStockOutDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdReferenceStockOutDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionReference(int id, UpdateReferenceStockOutDto referenceStockOutDto)
        {
            var transactionReference = _mapper.Map<ReferenceStockOut>(referenceStockOutDto);
            var response = await _referenceStockOutService.UpdateReferenceStockOutAsync(id, transactionReference);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedReferenceStockOutDto = _mapper.Map<ReferenceStockOutDto>(response.Data);
            return Ok(new ApiResponse<ReferenceStockOutDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedReferenceStockOutDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _referenceStockOutService.DeleteReferenceStockOutAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
