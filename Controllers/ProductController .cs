using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Services.ProductService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var response = await _productService.CreateProductAsync(product);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdProductDto = _mapper.Map<ProductDto>(response.Data);
            return CreatedAtAction("GetProduct", new { id = response.Data!.Id }, new ApiResponse<ProductDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdProductDto
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
