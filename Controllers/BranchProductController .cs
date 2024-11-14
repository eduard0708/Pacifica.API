using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Services.BranchProductService;

namespace Pacifica.API.Controllers
{
   // [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class BranchProductController : ControllerBase
    {
        private readonly IBranchProductService _branchProductService;
        private readonly IMapper _mapper;

        public BranchProductController(IBranchProductService branchProductService, IMapper mapper)
        {
            _branchProductService = branchProductService;
            _mapper = mapper;
        }

        // GET: api/BranchProduct/BranchId
        [HttpGet("GetAllByBranch/{branchId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>>> GetAllProductsByBranch(int branchId)
        {
            var response = await _branchProductService.GetAllProductsByBranchAsync(branchId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var branchProductDtos = _mapper.Map<IEnumerable<GetAllBranchProductResponseDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<GetAllBranchProductResponseDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchProductDtos
            });
        }

        // POST: api/BranchProduct/AddProduct
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ApiResponse<BranchProductResponseDto>>> AddProductToBranch([FromBody] BranchProductDto branchProductDto)
        {
            var branchProduct = _mapper.Map<BranchProduct>(branchProductDto);
            var response = await _branchProductService.AddProductToBranchAsync(branchProduct);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            // var addedBranchProductDto = _mapper.Map<BranchProductDto>(response.Data);
            return CreatedAtAction(nameof(GetAllProductsByBranch), new { branchId = branchProduct.BranchId }, new ApiResponse<BranchProductResponseDto>
            {
                Success = true,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("GetFilteredProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetBranchProductFilterSupplierCategorySKUDto>>>> GetFilteredProducts(
          [FromQuery] int branchId,
          [FromQuery] string? productCategory = null,
          [FromQuery] string? sku = null)
        {
            var response = await _branchProductService.GetProductsFilteredByBranchAsync(branchId, productCategory, sku);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
