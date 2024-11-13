using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionType;
using Pacifica.API.Helper;
using Pacifica.API.Services.TransactionTypeService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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

        // GET: api/TransactionType
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TransactionTypeDto>>>> GetTransactionTypes()
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

        // GET: api/TransactionType/5
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

        // POST: api/TransactionType
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionTypeDto>>> PostTransactionType(TransactionTypeDto transactionTypeDto)
        {
            var transactionType = _mapper.Map<TransactionType>(transactionTypeDto);
            var response = await _transactionTypeService.CreateTransactionTypeAsync(transactionType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdTransactionTypeDto = _mapper.Map<TransactionTypeDto>(response.Data);
            return CreatedAtAction("GetTransactionType", new { id = response.Data!.Id }, new ApiResponse<TransactionTypeDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdTransactionTypeDto
            });
        }

        // PUT: api/TransactionType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionType(int id, TransactionTypeDto transactionTypeDto)
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

        // DELETE: api/TransactionType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionType(int id)
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
