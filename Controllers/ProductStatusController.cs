using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Services.ProductStatusService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStatusController : ControllerBase
    {
        private readonly IProductStatusService _productStatusService;
        private readonly IMapper _mapper;

        public ProductStatusController(IProductStatusService productStatusService, IMapper mapper)
        {
            _productStatusService = productStatusService;
            _mapper = mapper;
        }

        // GET: api/TransactionReference
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductStatusDto>>>> GetTransactionType()
        {
            var response = await _productStatusService.GetAllProductStatusesAsync();
            var productStatusDtos = _mapper.Map<IEnumerable<ProductStatusDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<ProductStatusDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productStatusDtos
            });
        }

        // GET: api/TransactionReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductStatusDto>>> GetTransactionType(int id)
        {
            var response = await _productStatusService.GetProductStatusByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var productStatusDto = _mapper.Map<ProductStatusDto>(response.Data);
            return Ok(new ApiResponse<ProductStatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productStatusDto
            });
        }

        // POST: api/TransactionReference
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductStatusDto>>> CreateTransactionReference(CreateTransactionTypeDto transactionTypeDto)
        {
            var transactionType = _mapper.Map<ProductStatus>(transactionTypeDto);
            var response = await _productStatusService.CreateProductStatusAsync(transactionType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdProductStatusDto = _mapper.Map<ProductStatusDto>(response.Data);
            return CreatedAtAction("GetTransactionType", new { id = response.Data!.Id }, new ApiResponse<ProductStatusDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdProductStatusDto
            });
        }

        // PUT: api/TransactionReference/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductStatusAsync(int id, UpdateTransactionTypeDto transactionTypeDto)
        {
            var productStatus = _mapper.Map<ProductStatus>(transactionTypeDto);
            var response = await _productStatusService.UpdateProductStatusAsync(id, productStatus);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedProductStatusDto = _mapper.Map<ProductStatus>(response.Data);
            return Ok(new ApiResponse<ProductStatus>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedProductStatusDto
            });
        }

        // DELETE: api/TransactionReference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionReference(int id)
        {
            var response = await _productStatusService.DeleteProductStatusAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
