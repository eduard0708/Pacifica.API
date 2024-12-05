using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.StockInReferenceService;
using Pacifica.API.Models.Transaction;
using Pacifica.API.Dtos.StockInReference;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class StockInReferenceController : ControllerBase
    {
        private readonly IStockInReferenceService _StockInReferenceService;
        private readonly IMapper _mapper;

        public StockInReferenceController(IStockInReferenceService StockInReferenceService, IMapper mapper)
        {
            _StockInReferenceService = StockInReferenceService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockInReferenceDto>>>> GetStockInReference()
        {
            var response = await _StockInReferenceService.GetAllReferencesStockInAsync();
            var StockInReferenceDtos = _mapper.Map<IEnumerable<StockInReferenceDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<StockInReferenceDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = StockInReferenceDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockInReferenceDto>>> GetTransactionReference(int id)
        {
            var response = await _StockInReferenceService.GetReferencesStockInByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var StockInReferenceDto = _mapper.Map<StockInReferenceDto>(response.Data);
            return Ok(new ApiResponse<StockInReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = StockInReferenceDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockInReferenceDto>>> CreateTransactionReference(CreateStockInReferenceDto StockInReferenceDto)
        {
            var transactionReference = _mapper.Map<StockInReference>(StockInReferenceDto);
            var response = await _StockInReferenceService.CreateStockInReferenceAsync(transactionReference);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdStockInReferenceDto = _mapper.Map<StockInReferenceDto>(response.Data);
            return CreatedAtAction("GetTransactionReference", new { id = response.Data!.Id }, new ApiResponse<StockInReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdStockInReferenceDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionReference(int id, UpdateStockInReferenceDto StockInReferenceDto)
        {
            var transactionReference = _mapper.Map<StockInReference>(StockInReferenceDto);
            var response = await _StockInReferenceService.UpdateStockInReferenceAsync(id, transactionReference);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedStockInReferenceDto = _mapper.Map<StockInReferenceDto>(response.Data);
            return Ok(new ApiResponse<StockInReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedStockInReferenceDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _StockInReferenceService.DeleteStockInReferenceAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
