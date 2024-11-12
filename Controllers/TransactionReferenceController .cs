using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.TransactionReferenceService;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI

    public class TransactionReferenceController : ControllerBase
    {
        private readonly ITransactionReferenceService _transactionReferenceService;
        private readonly IMapper _mapper;

        public TransactionReferenceController(ITransactionReferenceService transactionReferenceService, IMapper mapper)
        {
            _transactionReferenceService = transactionReferenceService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TransactionReferenceDto>>>> GetTransactionReferences()
        {
            var response = await _transactionReferenceService.GetAllTransactionReferencesAsync();
            var transactionReferenceDtos = _mapper.Map<IEnumerable<TransactionReferenceDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<TransactionReferenceDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = transactionReferenceDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TransactionReferenceDto>>> GetTransactionReference(int id)
        {
            var response = await _transactionReferenceService.GetTransactionReferenceByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var transactionReferenceDto = _mapper.Map<TransactionReferenceDto>(response.Data);
            return Ok(new ApiResponse<TransactionReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = transactionReferenceDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionReferenceDto>>> PostTransactionReference(TransactionReferenceDto transactionReferenceDto)
        {
            var transactionReference = _mapper.Map<TransactionReference>(transactionReferenceDto);
            var response = await _transactionReferenceService.CreateTransactionReferenceAsync(transactionReference);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdTransactionReferenceDto = _mapper.Map<TransactionReferenceDto>(response.Data);
            return CreatedAtAction("GetTransactionReference", new { id = response.Data!.Id }, new ApiResponse<TransactionReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdTransactionReferenceDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionReference(int id, TransactionReferenceDto transactionReferenceDto)
        {
            var transactionReference = _mapper.Map<TransactionReference>(transactionReferenceDto);
            var response = await _transactionReferenceService.UpdateTransactionReferenceAsync(id, transactionReference);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedTransactionReferenceDto = _mapper.Map<TransactionReferenceDto>(response.Data);
            return Ok(new ApiResponse<TransactionReferenceDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedTransactionReferenceDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _transactionReferenceService.DeleteTransactionReferenceAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
