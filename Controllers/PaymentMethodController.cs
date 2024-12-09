using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.PaymentMethod;
using Pacifica.API.Models.Transaction;
using Pacifica.API.Services.BranchService;
using Pacifica.API.Services.PaymentMethodService;

namespace Pacifica.API.Controllers
{
   // [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper)
        {
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        // GET: api/Branch
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentMethodDto>>>> GetAllPaymentMethods()
        {
            var response = await _paymentMethodService.GetAllPaymentMethodsAsync();
            var paymentMethodDtos = _mapper.Map<IEnumerable<PaymentMethodDto>>(response.Data);
            return Ok(new ApiResponse<IEnumerable<PaymentMethodDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = paymentMethodDtos
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentMethodDto>>>> GetPaymentMethodByPage(
       [FromQuery] int? page = 1,
       [FromQuery] int? pageSize = 5,
       [FromQuery] string sortField = "paymentMethodName",  // Default sort field
       [FromQuery] int sortOrder = 1  // Default sort order (1 = ascending, -1 = descending)
   )
        {
            // Check if page or pageSize are not provided
            if (!page.HasValue || !pageSize.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<PaymentMethodDto>>
                {
                    Success = false,
                    Message = "Page and pageSize parameters are required."
                });
            }

            // Ensure page and pageSize are valid
            if (page < 1) return BadRequest(new ApiResponse<IEnumerable<PaymentMethodDto>>
            {
                Success = false,
                Message = "Page must be greater than or equal to 1."
            });

            if (pageSize < 1) return BadRequest(new ApiResponse<IEnumerable<PaymentMethodDto>>
            {
                Success = false,
                Message = "PageSize must be greater than or equal to 1."
            });

            // List of valid sort fields
            var validSortFields = new List<string> { "paymentMethodName", "description", "createdAt", "isDeleted" }; // Add more fields as needed

            if (!validSortFields.Contains(sortField))
            {
                return BadRequest(new ApiResponse<IEnumerable<PaymentMethodDto>>
                {
                    Success = false,
                    Message = "Invalid sort field.",
                    Data = null,
                    TotalCount = 0
                });
            }

            // Call service method to get branches by page, passing sortField and sortOrder
            var response = await _paymentMethodService.GetPaymentMethodByPageAsync(page.Value, pageSize.Value, sortField, sortOrder);

            // Map data to DTOs
            var paymentMethodDtos = _mapper.Map<IEnumerable<PaymentMethodDto>>(response.Data);

            return Ok(new ApiResponse<IEnumerable<PaymentMethodDto>>
            {
                Success = response.Success,
                Message = response.Message,
                Data = paymentMethodDtos,
                TotalCount = response.TotalCount
            });
        }

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> GetPaymentMethodById(int id)
        {
            var response = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            var paymentMethodDto = _mapper.Map<PaymentMethodDto>(response.Data);
            return Ok(new ApiResponse<PaymentMethodDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = paymentMethodDto
            });
        }

        // POST: api/Branch
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> CreatePaymentMethod(CreatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);
            var response = await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var createdPaymentMethod = _mapper.Map<PaymentMethodDto>(response.Data);
            return CreatedAtAction("GetBranch", new { id = response.Data!.Id }, new ApiResponse<PaymentMethodDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = createdPaymentMethod
            });
        }

        // PUT: api/Branch/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, UpdatePaymentMethodDto updatePaymentMethodDto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(updatePaymentMethodDto);
            var response = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethod);

            if (!response.Success)
            {
                return NotFound(response);
            }

            var paymentMethodDto = _mapper.Map<PaymentMethodDto>(response.Data);
            return Ok(new ApiResponse<PaymentMethodDto>
            {
                Success = response.Success,
                Message = response.Message,
                Data = paymentMethodDto
            });
        }

        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var response = await _paymentMethodService.DeletePaymentMethodAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }


    }
}
