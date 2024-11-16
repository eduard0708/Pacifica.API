using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockInOut;
using Pacifica.API.Services.StockInOutService;
namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockInOutController : ControllerBase
    {
        private readonly IStockInOutService _stockInOutService;
        private readonly IMapper _mapper;

        public StockInOutController(IStockInOutService stockInOutService, IMapper mapper)
        {
            _stockInOutService = stockInOutService;
            _mapper = mapper;
        }


        // POST: api/StockTransactionInOut/{transactionType}
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> ProcessTransaction(
            [FromBody] CreateStockInOutDto transaction)
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
                var response = await _stockInOutService.CreateStockInOutAsync(transaction);

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
        // [HttpPost("CreateMultiple")]
        // public async Task<ActionResult<ApiResponse<string>>> CreateStockInOut(
        //     [FromBody] List<CreateStockTransactionInOutDto> transactions)
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
        //         // Loop through each transaction in the list
        //         foreach (var transaction in transactions)
        //         {
        //             // Pass transactionType to the service method for each transaction
        //             var response = await _stockInOutService.CreateStockInOutAsync(transaction);

        //             if (!response.Success)
        //             {
        //                 return StatusCode(500, new ApiResponse<string>
        //                 {
        //                     Success = false,
        //                     Message = response.Message,
        //                     Data = null
        //                 });
        //             }
        //         }

        //         return Ok(new ApiResponse<string>
        //         {
        //             Success = true,
        //             Message = "Transactions processed successfully.",
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

        [HttpPost("CreateMultiple")]
        public async Task<ActionResult<ApiResponse<string>>> CreateStockInOut(
            [FromBody] List<CreateStockInOutDto> transactions)
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
                    // Check for the transaction existence and stock validity in the service
                    var response = await _stockInOutService.CreateStockInOutAsync(transaction);

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



        // GET: api/StockTransaction/GetAllTransactions
        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse<List<GetStockInOutDto>>>> GetAllTransactions()
        {
            try
            {
                var response = await _stockInOutService.GetAllTransactionsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<GetStockInOutDto>>
                {
                    Success = false,
                    Message = $"Error retrieving transactions: {ex.Message}",
                    Data = null
                });
            }
        }

        // GET: api/StockTransaction/GetByReferenceNumber/{referenceNumber}
        [HttpGet("GetByReferenceNumber/{referenceNumber}")]
        public async Task<ActionResult<ApiResponse<List<GetByReferenceNumberStockInOutDto>>>> GetByReferenceNumber(int referenceNumber)
        {
            try
            {
                // Call service method to retrieve the transaction by reference number
                var transaction = await _stockInOutService.GetTransactionByReferenceNumberAsync(referenceNumber);

                if (!transaction.Success)
                {
                    return NotFound(new ApiResponse<List<GetByReferenceNumberStockInOutDto>>
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
                return StatusCode(500, new ApiResponse<List<GetByReferenceNumberStockInOutDto>>
                {
                    Success = false,
                    Message = $"Error retrieving transaction: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}

