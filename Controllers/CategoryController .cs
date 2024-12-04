using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Category;
using Pacifica.API.Services.CategoryService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
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
        [HttpGet("all")]
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategoriesByPageAsync(
       [FromQuery] int? page,
       [FromQuery] int? pageSize,
       [FromQuery] string sortField = "categoryName",  // Default sort field
       [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
   )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "categoryName", "description", "createdAt", "isDeleted" }; // Add more fields as needed
            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get branches by page, passing sortField and sortOrder
            var response = await _categoryService.GetCategoriesByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var branchDtos = _mapper.Map<IEnumerable<CategoryDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchDtos,
                TotalCount = response.TotalCount
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
