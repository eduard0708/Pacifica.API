using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.ReferenceStockInService;
using Pacifica.API.Services.referenceStockInService;
using Pacifica.API.Dtos.ReferenceStockIn;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceStockInController : ControllerBase
    {
        private readonly IReferenceStockInService _referenceStockInService;
        private readonly IMapper _mapper;

        public ReferenceStockInController(IReferenceStockInService referenceStockInService, IMapper mapper)
        {
            _referenceStockInService = referenceStockInService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReferenceStockInDto>>>> GetReferenceStockIn()
        {
            var response = await _referenceStockInService.GetAllReferencesStockInAsync();
            var ReferenceStockInDtos = _mapper.Map<IEnumerable<ReferenceStockInDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<ReferenceStockInDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = ReferenceStockInDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReferenceStockInDto>>> GetTransactionReference(int id)
        {
            var response = await _referenceStockInService.GetReferencesStockInByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var ReferenceStockInDto = _mapper.Map<ReferenceStockInDto>(response.Data);
            return Ok(new ApiResponse<ReferenceStockInDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = ReferenceStockInDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReferenceStockInDto>>> CreateTransactionReference(CreateReferenceStockInDto ReferenceStockInDto)
        {
            var transactionReference = _mapper.Map<TransactionReference>(ReferenceStockInDto);
            var response = await _referenceStockInService.CreateReferenceStockInAsync(transactionReference);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdReferenceStockInDto = _mapper.Map<ReferenceStockInDto>(response.Data);
            return CreatedAtAction("GetTransactionReference", new { id = response.Data!.Id }, new ApiResponse<ReferenceStockInDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdReferenceStockInDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionReference(int id, UpdateReferenceStockInDto ReferenceStockInDto)
        {
            var transactionReference = _mapper.Map<TransactionReference>(ReferenceStockInDto);
            var response = await _referenceStockInService.UpdateReferenceStockInAsync(id, transactionReference);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedReferenceStockInDto = _mapper.Map<ReferenceStockInDto>(response.Data);
            return Ok(new ApiResponse<ReferenceStockInDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedReferenceStockInDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _referenceStockInService.DeleteReferenceStockInAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
