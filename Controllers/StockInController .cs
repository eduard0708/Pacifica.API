using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Services.StockInService;

namespace Pacifica.API.Controllers
{
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

        // GET: api/StockIn/reference/{referenceNumber}
        [HttpGet("reference/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockInDTO>>>> GetStockInByReferenceNumber(string referenceNumber)
        {
            var response = await _stockInService.GetStockInByReferenceNumberAsync(referenceNumber);

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

        // PUT: api/StockIn/reference/{referenceNumber}
        [HttpPut("reference/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<StockInDTO>>> UpdateStockInByReferenceNumber(string referenceNumber, StockInUpdateDTO stockInUpdateDto)
        {
            var response = await _stockInService.UpdateStockInByReferenceNumberAsync(referenceNumber, stockInUpdateDto);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK with updated data
            }

            return NotFound(response);  // Returns 404 if stock in record is not found
        }

        // DELETE: api/StockIn/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStockIn(int id)
        {
            var response = await _stockInService.DeleteStockInAsync(id);

            if (response.Success)
            {
                return Ok(response);  // Returns 200 OK if deletion was successful
            }

            return NotFound(response);  // Returns 404 if stock in record not found
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultipleStockInsAsync([FromBody] List<StockInUpdateDTO> stockInDtos)
        {
            if (stockInDtos == null || !stockInDtos.Any())
            {
                return BadRequest(new ApiResponse<List<StockInDTO>>
                {
                    Success = false,
                    Message = "No StockIn records provided for update.",
                    Data = null
                });
            }

            var response = await _stockInService.UpdateStockInsAsync(stockInDtos);

            if (response.Success)
            {
                return Ok(response); // Return the success response with the updated data
            }

            // Return the failed response with the list of failed updates and messages
            return BadRequest(response);
        }
    }
}
