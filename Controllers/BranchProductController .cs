using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.AuditTrails;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchProductResponseDto>>>> GetBranchProductsByPageAsync(
                [FromQuery] int? branchId, // Branch ID to filter products
                [FromQuery] int? page = 1,
                [FromQuery] int? pageSize = 5,
                [FromQuery] string sortField = "productName", // Default sort field
                [FromQuery] int sortOrder = 1)// Default sort order (1 = ascending, -1 = descending)
        {
            // Validate required parameters
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            if (page < 1)
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = "Page must be greater than or equal to 1."
                });
            }

            if (pageSize < 1)
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = "PageSize must be greater than or equal to 1."
                });
            }

            // List of valid sort fields
            var validSortFields = new List<string> { "productName","sku", "status", "costPrice", "retailPrice","stockQuantity",
                               "minStockLevel","reorderLevel", "createdAt" }; // Add fields relevant to BranchProduct
            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = $"Invalid sort field. Valid fields are: {string.Join(", ", validSortFields)}.",
                    Data = null
                });
            }

            // Fetch paginated and sorted products for the branch
            var response = await _branchProductService.GetBranchProductsByPageAsync(branchId!.Value, page.Value, pageSize.Value, sortField, sortOrder);

            if (!response.Success)
            {
                return NotFound(new ApiResponse<IEnumerable<BranchProductResponseDto>>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null,
                    TotalCount = 0
                });
            }

            // Map response data to DTOs
            var branchProductDtos = _mapper.Map<IEnumerable<BranchProductResponseDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<BranchProductResponseDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchProductDtos,
                TotalCount = response.TotalCount
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

//         [HttpPut("update/{id}")]
//         public async Task<ActionResult<ApiResponse<BranchProductResponseDto>>> UpdateBranchProduct(
//                 [FromRoute] int id, // Get the ID from the route
//                 [FromBody] UpdateBranchProductDto updateDto // Get the data from the request body
// )
//         {
//             var response = await _branchProductService.UpdateBranchProductAsync(updateDto);

//             if (!response.Success)
//             {
//                 return BadRequest(response); // Return bad request if failed
//             }

//             return Ok(response); // Return OK if successful
//         }


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


        [HttpGet("by-category-and-supplier")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchProductForStockInDTO>>>> GetBranchProductsByCategoryAndSupplier(
        [FromQuery] int branchId,
        [FromQuery] int categoryId,
        [FromQuery] int supplierId,
        [FromQuery] string sku = null!) // SKU parameter is optional
        {
            // If SKU is provided, search only by SKU
            if (!string.IsNullOrEmpty(sku))
            {
                var response = await _branchProductService.GetBranchProductsBySKUAsync(branchId, sku);

                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }

            // Otherwise, search by category and supplier
            var responseByCategoryAndSupplier = await _branchProductService.GetBranchProductsByCategoryAndSupplierAsync(branchId, categoryId, supplierId);

            if (!responseByCategoryAndSupplier.Success)
            {
                return NotFound(responseByCategoryAndSupplier);
            }

            return Ok(responseByCategoryAndSupplier);
        }

    }
}
