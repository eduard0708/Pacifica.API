using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Services.ProductService;

namespace Pacifica.API.Controllers
{
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
        [HttpGet]
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

        // GET: api/Product
        [HttpGet("DeletedProducts")]
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
        [HttpPost("CreateMultiple")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> CreateMultipleProducts([FromBody] List<CreateProductDto> productDtos)
        {
            // List to hold the created product DTOs
            var createdProducts = new List<ProductDto>();

            foreach (var productDto in productDtos)
            {
                // Map the DTO to the Product model
                var product = _mapper.Map<Product>(productDto);

                // Create the product via the service
                var response = await _productService.CreateProductAsync(product);

                if (!response.Success)
                {
                    // If any product fails to create, return an error response
                    return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                    {
                        Success = false,
                        Message = $"Failed to create product: {response.Message}",
                        Data = createdProducts
                    });
                }

                // Map the created product to a DTO and add it to the list
                var createdProductDto = _mapper.Map<ProductDto>(response.Data);
                createdProducts.Add(createdProductDto);
            }

            // Return the list of created products
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Message = "Products created successfully.",
                Data = createdProducts
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

        // DELETE: api/Product/5
        // DELETE: api/Product
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProducts([FromBody] ToDeletedProductsParam productsDelete)
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



        [HttpGet("GetFilteredProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetFilter_Products>>>> GetFilteredProducts(
              [FromQuery] string? category = null,
              [FromQuery] string? sku = null,
              [FromQuery] string? productName = null)
        {
            var response = await _productService.GetFilterProductsAsync(category, sku, productName);

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

    }
}
