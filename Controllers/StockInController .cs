using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Services.StockInService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [Route("api/[controller]")]
    [ApiController]
    public class StockInController : ControllerBase
    {
        private readonly IStockInService _stockInService;

        public StockInController(IStockInService stockInService)
        {
            _stockInService = stockInService;
        }

        // GET: api/StockIn
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockInDTO>>>> GetAllStockIns()
        {
            var response = await _stockInService.GetAllStockInsAsync();
            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if no records found
        }

        // GET: api/StockIn/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockInDTO>>> GetStockInById(int id)
        {
            var response = await _stockInService.GetStockInByIdAsync(id);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if stock in record is not found
        }

        // // GET: api/StockIn/reference/{referenceNumber}
        [HttpGet("reference/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockInDTO>>>> GetByReferenceNumber(string referenceNumber)
        {
            var response = await _stockInService.GetByReferenceNumber(referenceNumber);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with data
            }

            return NotFound(response);  // Returns 404 if stock in record is not found
        }

        // POST: api/StockIn
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockInDTO>>> CreateStockIn(StockInCreateDTO stockInCreateDto)
        {
            var response = await _stockInService.CreateStockInAsync(stockInCreateDto);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetStockInById), new { id = response.Data!.Id }, response);  // Returns 201 Created
            }

            return BadRequest(response);  // Returns 400 if creation failed
        }

        // POST: api/StockIn/multiple
        [HttpPost("multiple")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockInDTO>>>> CreateMultipleStockIns(IEnumerable<StockInCreateDTO> stockInCreateDtos)
        {
            var response = await _stockInService.CreateMultipleStockInAsync(stockInCreateDtos);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetStockInById), new { id = response.Data!.FirstOrDefault()?.Id }, response);  // Returns 201 Created
            }

            return BadRequest(response);  // Returns 400 if creation failed
        }

        // PUT: api/StockIn/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StockInDTO>>> UpdateStockIn(int id, StockInUpdateDTO stockInUpdateDto)
        {
            var response = await _stockInService.UpdateStockInAsync(id, stockInUpdateDto);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with updated data
            }

            return NotFound(response);  // Returns 404 if stock in record is not found
        }

        // DELETE: api/StockIn/5
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStockIn([FromBody] StockInDeleteParams deleteParams)
        {
            var response = await _stockInService.DeleteStockInAsync(deleteParams);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK if deletion was successful
            }

            return NotFound(response);  // Returns 404 if stock in record not found
        }

        [HttpPost("restore")]
        public async Task<ActionResult<ApiResponse<bool>>> RestoreStockIn([FromBody] StockInRestoreParams restoreParams)
        {
            // Call the service method to restore the StockIn record.
            var response = await _stockInService.RestoreStockInAsync(restoreParams);

            if (!response.Success)
            {
                return NotFound(response);  // Return 404 if not found or failed.
            }

            return Ok(response);  // Return 200 with the success response.
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<ApiResponse<List<StockIn>>>> GetAllDeletedStockIn()
        {
            // Call the service method to get all deleted StockIn records.
            var response = await _stockInService.GetAllDeletedStockInAsync();

            if (!response.Success)
            {
                return NotFound(response);  // Return 404 if no deleted records found.
            }

            return Ok(response);  // Return 200 with the list of deleted StockIn records.
        }

        // GET: api/StockIn/reference/{referenceNumber}
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ViewStockInDTO>>>> GetByDateRangeOrRefenceAsync(
            [FromQuery] string? referenceNumber = null,  // Make referenceNumber optional
            [FromQuery] DateTime? dateCreatedStart = null,
            [FromQuery] DateTime? dateCreatedEnd = null,
            [FromQuery] DateTime? dateReportedStart = null,
            [FromQuery] DateTime? dateReportedEnd = null)
        {
            // Debug log to check received parameters
            Console.WriteLine($"referenceNumber: {referenceNumber}, dateCreatedStart: {dateCreatedStart}, dateCreatedEnd: {dateCreatedEnd}");

            var response = await _stockInService.GetByDateRangeOrRefenceAsync(
                referenceNumber!,
                dateCreatedStart,
                dateCreatedEnd,
                dateReportedStart,
                dateReportedEnd
            );

            if (response.Success)
            {
                return Ok(response);  // Return 200 OK with data
            }

            return NotFound(response);  // Return 404 if no data found
        }

    }
}
