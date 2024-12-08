using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Supplier;
using Pacifica.API.Services.SupplierService;

namespace Pacifica.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        // GET: api/Supplier
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetSuppliers()
        {
            var response = await _supplierService.GetAllSuppliersAsync();
            var supplierDtos = _mapper.Map<IEnumerable<SupplierDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<SupplierDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = supplierDtos
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetSuppliersByPageAsync(
               [FromQuery] int? page = 1,
               [FromQuery] int? pageSize = 5,
               [FromQuery] string sortField = "supplierName",  // Default sort field
               [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
           )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<SupplierDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<SupplierDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<SupplierDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "supplierName", "contactPerson", "contactNumber", "isDeleted" }; // Add more fields as needed

            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<SupplierDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get branches by page, passing sortField and sortOrder
            var response = await _supplierService.GetSuppliersByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var branchDtos = _mapper.Map<IEnumerable<SupplierDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<SupplierDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = branchDtos,
                TotalCount = response.TotalCount
            });
        }

        // GET: api/Supplier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> GetSupplier(int id)
        {
            var response = await _supplierService.GetSupplierByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var supplierDto = _mapper.Map<SupplierDto>(response.Data);
            return Ok(new ApiResponse<SupplierDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = supplierDto
            });
        }

     // GET: supplier for seelction dropdown list
        [HttpGet("select")]
        public async Task<ActionResult<ApiResponse<SelectSupplierDTO>>> GetSelectSupplier([FromQuery]int id)
        {
            var response = await _supplierService.GetSelectSuppliersAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var supplierDto = _mapper.Map<SelectSupplierDTO>(response.Data);
            return Ok(new ApiResponse<SelectSupplierDTO>
            {
                Success = response.Success,
                Message = response.Message,
                Data = supplierDto
            });
        }

            // GET: supplier for seelction dropdown list
        [HttpGet("suppler-by-category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SelectSupplierDTO>>>> GetSuppliersByCategory(int categoryId)
        {
            var response = await _supplierService.GetSuppliersByCategory(categoryId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(new ApiResponse<IEnumerable<SelectSupplierDTO>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        // POST: api/Supplier
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> CreateSupplier([FromBody] CreateSupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            var response = await _supplierService.CreateSupplierAsync(supplier);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdSupplierDto = _mapper.Map<SupplierDto>(response.Data);
            return CreatedAtAction(nameof(GetSupplier), new { id = response.Data!.Id }, new ApiResponse<SupplierDto>
            {
                Success = true,
                Message = response.Message,
                Data = createdSupplierDto
            });
        }

        // PUT: api/Supplier/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> UpdateSupplier(int id, [FromBody] UpdateSupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            var response = await _supplierService.UpdateSupplierAsync(id, supplier);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var updatedSupplierDto = _mapper.Map<SupplierDto>(response.Data);
            return Ok(new ApiResponse<SupplierDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = updatedSupplierDto
            });
        }

        // DELETE: api/Supplier/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSupplier(int id)
        {
            var response = await _supplierService.DeleteSupplierAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(new ApiResponse<bool>
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}
