using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.TransactionTypeService;

namespace Pacifica.API.Controllers
{
  //  [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IMapper _mapper;

        public TransactionTypeController(ITransactionTypeService transactionTypeService, IMapper mapper)
        {
            _transactionTypeService = transactionTypeService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TransactionTypeDto>>>> GetTransactionType()
        {
            var response = await _transactionTypeService.GetAllTransactionTypesAsync();
            var transactionTypeDtos = _mapper.Map<IEnumerable<TransactionTypeDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<TransactionTypeDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = transactionTypeDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TransactionTypeDto>>> GetTransactionType(int id)
        {
            var response = await _transactionTypeService.GetTransactionTypeByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var transactionTypeDto = _mapper.Map<TransactionTypeDto>(response.Data);
            return Ok(new ApiResponse<TransactionTypeDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = transactionTypeDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionTypeDto>>> CreateTransactionReference(CreateTransactionTypeDto transactionTypeDto)
        {
            var transactionType = _mapper.Map<TransactionType>(transactionTypeDto);
            var response = await _transactionTypeService.CreateTransactionTypeAsync(transactionType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdTransactionReferenceDto = _mapper.Map<TransactionTypeDto>(response.Data);
            return CreatedAtAction("GetTransactionType", new { id = response.Data!.Id }, new ApiResponse<TransactionTypeDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdTransactionReferenceDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionType(int id, UpdateTransactionTypeDto transactionTypeDto)
        {
            var transactionType = _mapper.Map<TransactionType>(transactionTypeDto);
            var response = await _transactionTypeService.UpdateTransactionTypeAsync(id, transactionType);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedTransactionTypeDto = _mapper.Map<TransactionTypeDto>(response.Data);
            return Ok(new ApiResponse<TransactionTypeDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedTransactionTypeDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _transactionTypeService.DeleteTransactionTypeAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
