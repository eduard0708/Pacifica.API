using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Services.StockOutReferenceService;  // Updated service
using Pacifica.API.Models.Transaction;
using Pacifica.API.Dtos.StockOutReference;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockOutReferenceController : ControllerBase
    {
        private readonly IStockOutReferenceService _stockOutReferenceService;  // Updated service
        private readonly IMapper _mapper;

        public StockOutReferenceController(IStockOutReferenceService stockOutReferenceService, IMapper mapper)
        {
            _stockOutReferenceService = stockOutReferenceService;  // Updated service
            _mapper = mapper;
        }

        // GET: api/StockOutReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockOutReferenceDto>>>> GetStockOutReference()
        {
            var response = await _stockOutReferenceService.GetAllReferencesStockOutAsync();  // Updated service call
            var stockOutReferenceDtos = _mapper.Map<IEnumerable<StockOutReferenceDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<StockOutReferenceDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = stockOutReferenceDtos
            });
        }

        // GET: api/StockOutReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockOutReferenceDto>>> GetStockOutReferenceById(int id)  // Updated endpoint name
        {
            var response = await _stockOutReferenceService.GetReferencesStockOutByIdAsync(id);  // Updated service call
            if (!response.Success)
            {
                return NotFound(response);
            }

            var stockOutReferenceDto = _mapper.Map<StockOutReferenceDto>(response.Data);
            return Ok(new ApiResponse<StockOutReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = stockOutReferenceDto
            });
        }

        // POST: api/StockOutReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockOutReferenceDto>>> CreateStockOutReference(CreateStockOutReferenceDto stockOutReferenceDto)  // Updated DTO
        {
            var stockOutReference = _mapper.Map<StockOutReference>(stockOutReferenceDto);  // Updated mapping
            var response = await _stockOutReferenceService.CreateStockOutReferenceAsync(stockOutReference);  // Updated service call

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdStockOutReferenceDto = _mapper.Map<StockOutReferenceDto>(response.Data);
            return CreatedAtAction("GetStockOutReferenceById", new { id = response.Data!.Id }, new ApiResponse<StockOutReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdStockOutReferenceDto
            });
        }

        // PUT: api/StockOutReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockOutReference(int id, UpdateStockOutReferenceDto stockOutReferenceDto)  // Updated DTO
        {
            var stockOutReference = _mapper.Map<StockOutReference>(stockOutReferenceDto);  // Updated mapping
            var response = await _stockOutReferenceService.UpdateStockOutReferenceAsync(id, stockOutReference);  // Updated service call

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedStockOutReferenceDto = _mapper.Map<StockOutReferenceDto>(response.Data);
            return Ok(new ApiResponse<StockOutReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedStockOutReferenceDto
            });
        }

        // DELETE: api/StockOutReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockOutReference(int id)
        {
            var response = await _stockOutReferenceService.DeleteStockOutReferenceAsync(id);  // Updated service call
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
