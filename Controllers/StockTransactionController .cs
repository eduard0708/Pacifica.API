using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockTransactionInOut;
using Pacifica.API.Services.StockTransactionService;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionService _stockTransactionService;

        public StockTransactionController(IStockTransactionService stockTransactionService)
        {
            _stockTransactionService = stockTransactionService;
        }

        [HttpPost("ProcessTransaction")]
        public async Task<ActionResult<ApiResponse<string>>> ProcessTransaction([FromBody] CreateStockTransactionInOutDto transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid transaction data.",
                    Data = null
                });
            }

            try
            {
                // Call the service method, which now returns an ApiResponse
                var response = await _stockTransactionService.ProcessTransactionAsync(transaction);

                if (!response.Success)
                {
                    return StatusCode(500, new ApiResponse<string>
                    {
                        Success = false,
                        Message = response.Message,
                        Data = null
                    });
                }

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Transaction processed successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
