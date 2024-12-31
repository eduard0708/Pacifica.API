using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Services.ProductService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: api/Product
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProducts()
        {
            var response = await _productService.GetAllProductsAsync();

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productDtos
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsByPageAsync(
                [FromQuery] int? page = 1,
                [FromQuery] int? pageSize = 5,
                [FromQuery] string sortField = "productName",  // Default sort field
                [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
            )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "productName", "category", "sku", "supplier", "remarks", "createdAt", "isDeleted" }; // Add more fields as needed

            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get products by page, passing sortField and sortOrder
            var response = await _productService.GetProductsByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productDtos,
                TotalCount = response.TotalCount
            });
        }

        // GET: api/Product
        [HttpGet("GetAllDeleted")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeletetedProductsDto>>>> GetDeteltedProducts()
        {
            var response = await _productService.GetAllDeletedProductsAsync();
            var productDtos = _mapper.Map<IEnumerable<DeletetedProductsDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<DeletetedProductsDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productDtos
            });
        }

        [HttpGet("GetDeleted/{productId}")]
        public async Task<ActionResult<ApiResponse<DeletetedProductsDto>>> GetDeletedProducts(int productId)
        {
            var response = await _productService.GetDeletedProductByIdAsync(productId);

            if (!response.Success)
            {
                // Return failure response when product is not found
                return Ok(new ApiResponse<DeletetedProductsDto>
                {
                    Success = response.Success,
                    Message = response.Message,
                    Data = null
                });
            }

            // Map the Product entity to the DeletetedProductsDto
            var productDtos = _mapper.Map<DeletetedProductsDto>(response.Data);

            // Return the success response with the mapped DTO
            return Ok(new ApiResponse<DeletetedProductsDto>
            {
                Success = true,
                Message = "Deleted product retrieved successfully.",
                Data = productDtos
            });
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var productDto = _mapper.Map<ProductDto>(response.Data);
            return Ok(new ApiResponse<ProductDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = productDto
            });
        }

        // POST: api/Product/CreateMultiple
        [HttpPost("multiple")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> CreateMultipleProducts([FromBody] List<CreateProductDto> productDtos)
        {

            // Create the product via the service
            var response = await _productService.CreateMulipleProductsAsync(productDtos);

            if (!response.Success)
            {
                // If any product fails to create, return an error response
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = $"Failed to create product: {response.Message}",
                    Data = null
                });
            }


            // Return the list of created products
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Message = "Products created successfully.",
                Data = response.Data
            });
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto productDtos)
        {

            // Create the product via the service
            var response = await _productService.CreateProductAsync(productDtos);

            if (!response.Success)
            {
                // If any product fails to create, return an error response
                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = $"Failed to create product: {response.Message}",
                    Data = null
                });
            }


            // Return the list of created products
            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Message = "Products created successfully.",
                Data = response.Data
            });
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var response = await _productService.UpdateProductAsync(id, product);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedProductDto = _mapper.Map<ProductDto>(response.Data);
            return Ok(new ApiResponse<ProductDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedProductDto
            });
        }

        // DELETE: api/Product
        [HttpDelete]

        public async Task<ActionResult<ApiResponse<object>>> DeleteProducts([FromBody] DeletedProductsParam productsDelete)
        {
            // Call the service to delete products
            var response = await _productService.DeleteProductsAsync(productsDelete);

            // If the operation failed, return a NotFound response with the error message
            if (!response.Success)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            // Return a success response with a success message
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Products successfully deleted!",
                Data = null
            });
        }

        // GET: api/ get by category sku or productname
        // FrontEndUse - Stock-In -
        [HttpGet("GetFilteredProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetFilter_Products>>>> GetFilteredProducts([FromQuery] ProductFilterParams productFilter)
        {
            var response = await _productService.GetFilterProductsAsync(productFilter);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET: api/Product/AuditDetails/5
        [HttpGet("AuditDetails/{id}")]
        public async Task<ActionResult<ApiResponse<List<AuditDetails>>>> GetAuditDetails(int id)
        {
            var response = await _productService.GetProductAuditDetailsAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("restore-deleted")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> RestoreDeletedProducts([FromBody] RestoreDeletedProductsParam deletedProducts)
        {
            if (deletedProducts == null || deletedProducts.ProductIds == null || !deletedProducts.ProductIds.Any())
            {
                return BadRequest(new ApiResponse<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "Product IDs are required to restore deleted products.",
                    Data = null
                });
            }

            var response = await _productService.RestoreDeletedProductsAsync(deletedProducts);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET: api/Product/AuditDetails/5
        [HttpGet("ProductAuditTrails/{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductAuditTrailsDto>>>> GetProductAuditTraisl(int productId)
        {
            var response = await _productService.GetProductAuditTrailsAsync(productId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
