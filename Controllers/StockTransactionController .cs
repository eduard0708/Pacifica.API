using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockTransactionInOut;
using Pacifica.API.Services.StockTransactionServiceInOut;
namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionServiceInOut _stockTransactionService;
        private readonly IMapper _mapper;

        public StockTransactionController(IStockTransactionServiceInOut stockTransactionService, IMapper mapper)
        {
            _stockTransactionService = stockTransactionService;
            _mapper = mapper;
        }


        // POST: api/StockTransactionInOut/{transactionType}
        [HttpPost("StockTransactionInOut/{transactionType}")]
        public async Task<ActionResult<ApiResponse<string>>> ProcessTransaction(
            [FromRoute] int transactionType,
            [FromBody] CreateStockTransactionInOutDto transaction)
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
                // Pass transactionType to the service method if required
                var response = await _stockTransactionService.ProcessTransactionAsync(transaction, transactionType);

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

        // POST: api/StockTransactionInOut/{transactionType}
        [HttpPost("AddListOfTransactionInOut/{transactionType}")]
        public async Task<ActionResult<ApiResponse<string>>> AddListOfTransactionInOut(
            [FromRoute] int transactionType,
            [FromBody] List<CreateStockTransactionInOutDto> transactions)
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
                // Loop through each transaction in the list
                foreach (var transaction in transactions)
                {
                    // Pass transactionType to the service method for each transaction
                    var response = await _stockTransactionService.ProcessTransactionAsync(transaction, transactionType);

                    if (!response.Success)
                    {
                        return StatusCode(500, new ApiResponse<string>
                        {
                            Success = false,
                            Message = response.Message,
                            Data = null
                        });
                    }
                }

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Transactions processed successfully.",
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


        // // POST: api/StockTransactionInOut
        // [HttpPost("StockTransactionInOut")]
        // public async Task<ActionResult<ApiResponse<string>>> ProcessTransaction([FromBody] CreateStockTransactionInOutDto transaction)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(new ApiResponse<string>
        //         {
        //             Success = false,
        //             Message = "Invalid transaction data.",
        //             Data = null
        //         });
        //     }

        //     try
        //     {
        //         var response = await _stockTransactionService.ProcessTransactionAsync(transaction);

        //         if (!response.Success)
        //         {
        //             return StatusCode(500, new ApiResponse<string>
        //             {
        //                 Success = false,
        //                 Message = response.Message,
        //                 Data = null
        //             });
        //         }

        //         return Ok(new ApiResponse<string>
        //         {
        //             Success = true,
        //             Message = "Transaction processed successfully.",
        //             Data = null
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new ApiResponse<string>
        //         {
        //             Success = false,
        //             Message = ex.Message,
        //             Data = null
        //         });
        //     }
        // }

        // GET: api/StockTransaction/GetAllTransactions
        [HttpGet("GetAllTransactions")]
        public async Task<ActionResult<ApiResponse<List<GetStockTransactionInOutDto>>>> GetAllTransactions()
        {
            try
            {
                var response = await _stockTransactionService.GetAllTransactionsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<GetStockTransactionInOutDto>>
                {
                    Success = false,
                    Message = $"Error retrieving transactions: {ex.Message}",
                    Data = null
                });
            }
        }

        // GET: api/StockTransaction/GetByReferenceNumber/{referenceNumber}
        [HttpGet("GetByReferenceNumber/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>>> GetByReferenceNumber(int referenceNumber)
        {
            try
            {
                // Call service method to retrieve the transaction by reference number
                var transaction = await _stockTransactionService.GetTransactionByReferenceNumberAsync(referenceNumber);

                if (!transaction.Success)
                {
                    return NotFound(new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
                    {
                        Success = false,
                        Message = "Transaction not found.",
                        Data = null
                    });
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<GetByReferenceNumberStockTransactionInOutDto>>
                {
                    Success = false,
                    Message = $"Error retrieving transaction: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}

