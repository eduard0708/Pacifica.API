using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Services.BranchProductService;

namespace Pacifica.API.Controllers
{
     //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchProductResponseDto>>>> AddProductsToBranch([FromBody] IEnumerable<AddProductToBranchDto> branchProductDtos)
        {
            // Map DTO list to BranchProduct list
            var branchProducts = _mapper.Map<IEnumerable<BranchProduct>>(branchProductDtos);

            // Call the service to add all products at once
            var response = await _branchProductService.AddProductsToBranchAsync(branchProducts);

            if (!response.Success)
            {
                // If any product fails to be added, return a bad request with the error message
                return BadRequest(response);
            }

            // If all products are added successfully, return them in the response
            return CreatedAtAction(nameof(GetAllProductsByBranch),
                new { branchId = branchProducts.FirstOrDefault()?.BranchId },
                new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = true,
                    Message = "Products added successfully",
                    Data = response.Data
                });
        }

        [HttpGet("GetFilteredProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetBranchProductFilterDto>>>> GetFilteredProducts(
          [FromQuery] int branchId,
          [FromQuery] string? productCategory = null,
          [FromQuery] string? sku = null,
          [FromQuery] string? productName = null)
        {
            var response = await _branchProductService.GetProductsFilteredByBranchAsync(branchId, productCategory, sku, productName);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // PUT: api/BranchProduct/UpdateProduct/5
        [HttpPut("Update/{branchId}/{productId}")]
        public async Task<ActionResult<ApiResponse<BranchProductResponseDto>>> UpdateBranchProduct(int branchId, int productId, [FromBody] UpdateBranchProductDto updateDto)
        {
            var response = await _branchProductService.UpdateBranchProductAsync(branchId, productId, updateDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE: api/BranchProduct/DeleteProduct/5
        [HttpDelete("Delete/{branchId}/{productId}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDeleteBranchProduct(int branchId, int productId)
        {
            // Call the service method with both branchId and productId
            var response = await _branchProductService.SoftDeleteBranchProductAsync(branchId, productId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }

}

