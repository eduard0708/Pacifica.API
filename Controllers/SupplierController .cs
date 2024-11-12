using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Supplier;
using Pacifica.API.Services.SupplierService;

namespace Pacifica.API.Controllers
{
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
        [HttpGet]
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

        // POST: api/Supplier
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> CreateSupplier([FromBody] SupplierDto supplierDto)
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
        public async Task<ActionResult<ApiResponse<SupplierDto>>> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
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
