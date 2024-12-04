using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Services.BranchProductService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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


        [HttpGet("get-allby-branch/{branchId}")]
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


        [HttpGet("{branchId}/{productId}")]
        public async Task<ActionResult<ApiResponse<BranchProductResponseDto>>> GetAllProductsByBranch(int branchId, int productId)
        {
            var response = await _branchProductService.GetProductsInBranchAsync(branchId, productId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            // var branchProductDtos = _mapper.Map<BranchProductResponseDto>>(response.Data);

            return Ok(new ApiResponse<BranchProductResponseDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }


        [HttpPost("add-product")]
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

        [HttpGet("get-filtered")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetBranchProductFilterDto>>>> GetFilteredBranchProducts([FromQuery] FilterBranchProductsParams filter)
        {
            var response = await _branchProductService.GetFilteredBranchProductsAsync(filter);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<BranchProductResponseDto>>> UpdateBranchProduct([FromBody] UpdateBranchProductDto updateDto)
        {
            var response = await _branchProductService.UpdateBranchProductAsync(updateDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDeleteBranchProduct([FromQuery] SoftDeleteBranchProductParams deleteBranchProduct)
        {
            // Call the service method with the parameters
            var response = await _branchProductService.SoftDeleteBranchProductAsync(deleteBranchProduct);

            // Return appropriate response based on the service method result
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("branchproduct-audit-trails/{branchId}/{productId}")]
        public async Task<ActionResult<ApiResponse<List<BranchProductAuditTrailsDto>>>> GetBranchProductAuditTraisl(int branchId, int productId)
        {
            var response = await _branchProductService.GetBranchProductAuditTrailsAsync(branchId, productId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetAllDeleted")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchProductDto>>>> GetDeteltedBranchProducts()
        {
            var response = await _branchProductService.GetAllDeletedBranchProductsAsync();

            return Ok(new ApiResponse<IEnumerable<BranchProductDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }


        [HttpPost("RestoreDeleted")]
        public async Task<ActionResult<ApiResponse<List<int>>>> RestoreDeletedProducts([FromBody] RestoreDeletedBranchProductsParams restoreDeleted)
        {

            var response = await _branchProductService.RestoreDeletedBrachProductsAsync(restoreDeleted);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }



    }

}

