using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Category;
using Pacifica.API.Services.CategoryService;

namespace Pacifica.API.Controllers
{
   // [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = categoryDtos
            });
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var categoryDto = _mapper.Map<CategoryDto>(response.Data);
            return Ok(new ApiResponse<CategoryDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = categoryDto
            });
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var response = await _categoryService.CreateCategoryAsync(category);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdCategoryDto = _mapper.Map<CategoryDto>(response.Data);
            return CreatedAtAction("GetCategory", new { id = response.Data!.Id }, new ApiResponse<CategoryDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdCategoryDto
            });
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var response = await _categoryService.UpdateCategoryAsync(id, category);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var updatedCategoryDto = _mapper.Map<CategoryDto>(response.Data);
            return Ok(new ApiResponse<CategoryDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedCategoryDto
            });
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
