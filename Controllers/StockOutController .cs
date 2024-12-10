using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockOut;  // Change StockIn DTOs to StockOut DTOs
using Pacifica.API.Models.Transaction;
using Pacifica.API.Services.StockOutService;  // Change to StockOutService

namespace Pacifica.API.Controllers
{
  //  [ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class StockOutController : ControllerBase  // Rename the controller to StockOutController
    {
        private readonly IStockOutService _stockOutService;  // Change to StockOutService

        public StockOutController(IStockOutService stockOutService)  // Inject StockOutService
        {
            _stockOutService = stockOutService;
        }

        // GET: api/StockOut
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockOutDTO>>>> GetAllStockOuts()  // Change StockIn to StockOut
        {
            var response = await _stockOutService.GetAllStockOutsAsync();  // Call StockOutService method

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if no records found
        }

        // GET: api/StockOut/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockOutDTO>>> GetStockOutById(int id)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.GetStockOutByIdAsync(id);  // Call StockOutService method

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if stock out record is not found
        }

        // GET: api/StockOut/reference/{referenceNumber}
        [HttpGet("reference/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockOutDTO>>>> GetStockOutByReferenceNumber(string referenceNumber)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.GetStockOutByReferenceNumberAsync(referenceNumber);  // Call StockOutService method

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if stock out record is not found
        }

        // POST: api/StockOut
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockOutDTO>>> CreateStockOut(CreateStockOutDTO stockOutCreateDto)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.CreateStockOutAsync(stockOutCreateDto);  // Call StockOutService method

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetStockOutById), new { id = response.Data!.Id }, response);  // Returns 201 Created
            }

            return BadRequest(response);  // Returns 400 if creation failed
        }

        // POST: api/StockOut/multiple
        [HttpPost("multiple")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockOutDTO>>>> CreateMultipleStockOuts(IEnumerable<CreateStockOutDTO> stockOutCreateDtos)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.CreateMultipleStockOutAsync(stockOutCreateDtos);  // Call StockOutService method

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetStockOutById), new { id = response.Data!.FirstOrDefault()?.Id }, response);  // Returns 201 Created
            }

            return BadRequest(response);  // Returns 400 if creation failed
        }

        // PUT: api/StockOut/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StockOutDTO>>> UpdateStockOut(int id, StockOutUpdateDTO stockOutUpdateDto)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.UpdateStockOutAsync(id, stockOutUpdateDto);  // Call StockOutService method

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with updated data
            }

            return NotFound(response);  // Returns 404 if stock out record is not found
        }

        // DELETE: api/StockOut/5
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStockOut([FromBody] StockOutDeleteParams deleteParams)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.DeleteStockOutAsync(deleteParams);  // Call StockOutService method

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK if deletion was successful
            }

            return NotFound(response);  // Returns 404 if stock out record not found
        }

        [HttpPost("restore")]
        public async Task<ActionResult<ApiResponse<bool>>> RestoreStockOut([FromBody] StockOutRestoreParams restoreParams)  // Change StockIn to StockOut
        {
            var response = await _stockOutService.RestoreStockOutAsync(restoreParams);  // Call StockOutService method

            if (!response.Success)
            {
                return NotFound(response);  // Return 404 if not found or failed
            }

            return Ok(response);  // Return 200 with the success response
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<ApiResponse<List<StockOut>>>> GetAllDeletedStockOut()  // Change StockIn to StockOut
        {
            var response = await _stockOutService.GetAllDeletedStockOutAsync();  // Call StockOutService method

            if (!response.Success)
            {
                return NotFound(response);  // Return 404 if no deleted records found
            }

            return Ok(response);  // Return 200 with the list of deleted StockOut records
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ViewStockOutDTO>>>> GetByDateRangeOrRefenceAsync(
        [FromQuery] string? referenceNumber = null,  // Make referenceNumber optional
        [FromQuery] DateTime? dateCreatedStart = null,
        [FromQuery] DateTime? dateCreatedEnd = null,
        [FromQuery] DateTime? dateSoldStart = null,
        [FromQuery] DateTime? dateSoldeEnd = null)
        {

            var response = await _stockOutService.GetByDateRangeOrRefenceAsync(
                referenceNumber!,
                dateCreatedStart,
                dateCreatedEnd,
                dateSoldStart,
                dateSoldeEnd
            );

            if (response.Success)
            {
                return Ok(response);  // Return 200 OK with data
            }

            return NotFound(response);  // Return 404 if no data found
        }
    }
}

