using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.StockTransactionInOut;
using Pacifica.API.Services.StockTransactionServiceInout;
using Pacifica.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // POST: api/StockTransactionInOut
        [HttpPost("StockTransactionInOut")]
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




// using Microsoft.AspNetCore.Mvc;
// using Pacifica.API.Dtos.StockTransactionInOut;
// using Pacifica.API.Services.StockTransactionServiceInout;

// namespace Pacifica.API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class StockTransactionController : ControllerBase
//     {
//         private readonly IStockTransactionServiceInOut _stockTransactionService;

//         public StockTransactionController(IStockTransactionServiceInOut stockTransactionService)
//         {
//             _stockTransactionService = stockTransactionService;
//         }

//         [HttpPost("StockTransactionInOut")]
//         public async Task<ActionResult<ApiResponse<string>>> ProcessTransaction([FromBody] CreateStockTransactionInOutDto transaction)

//         {
//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(new ApiResponse<string>
//                 {
//                     Success = false,
//                     Message = "Invalid transaction data.",
//                     Data = null
//                 });
//             }

//             try
//             {
//                 // Call the service method, which now returns an ApiResponse
//                 var response = await _stockTransactionService.ProcessTransactionAsync(transaction);

//                 if (!response.Success)
//                 {
//                     return StatusCode(500, new ApiResponse<string>
//                     {
//                         Success = false,
//                         Message = response.Message,
//                         Data = null
//                     });
//                 }

//                 return Ok(new ApiResponse<string>
//                 {
//                     Success = true,
//                     Message = "Transaction processed successfully.",
//                     Data = null
//                 });
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new ApiResponse<string>
//                 {
//                     Success = false,
//                     Message = ex.Message,
//                     Data = null
//                 });
//             }
//         }

//         [HttpGet("GetAllTransactions")]
//         public async Task<ActionResult<ApiResponse<List<GetStockTransactionInOutDto>>>> GetAllTransactions()
//         {
//             try
//             {
//                 var response = await _stockTransactionService.GetAllTransactionsAsync();
//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new ApiResponse<List<GetStockTransactionInOutDto>>
//                 {
//                     Success = false,
//                     Message = $"Error retrieving transactions: {ex.Message}",
//                     Data = null
//                 });
//             }
//         }


//         // New method for retrieving a transaction by reference number (enum comparison)
//         [HttpGet("GetByReferenceNumber/{referenceNumber}")]
//         public async Task<ActionResult<ApiResponse<GetStockTransactionInOutDto>>> GetByReferenceNumber(int referenceNumber)
//         {
//             try
//             {
//                 var transaction = await _context.StockTransactionInOuts
//                     .FirstOrDefaultAsync(t => (int)t.TransactionReference == referenceNumber);  // Assuming TransactionReference is an enum

//                 if (transaction == null)
//                 {
//                     return NotFound(new ApiResponse<GetStockTransactionInOutDto>
//                     {
//                         Success = false,
//                         Message = "Transaction not found.",
//                         Data = null
//                     });
//                 }

//                 // Map the transaction entity to the DTO to return it
//                 var transactionDto = new GetStockTransactionInOutDto
//                 {
//                     TransactionId = transaction.TransactionId,
//                     BranchId = transaction.BranchId,
//                     ProductId = transaction.ProductId,
//                     TransactionType = transaction.TransactionType,
//                     StockQuantity = transaction.StockQuantity,
//                     TransactionDate = transaction.TransactionDate
//                 };

//                 return Ok(new ApiResponse<GetStockTransactionInOutDto>
//                 {
//                     Success = true,
//                     Message = "Transaction retrieved successfully.",
//                     Data = transactionDto
//                 });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new ApiResponse<GetStockTransactionInOutDto>
//                 {
//                     Success = false,
//                     Message = $"Error retrieving transaction: {ex.Message}",
//                     Data = null
//                 });
//             }
//         }
//     }
// }


