using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Services;
using Pacifica.API.Models.Reports.F152Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pacifica.API.Services.F152ReportService;
using Pacifica.API.Dtos.F152Report;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class F152ReportController : ControllerBase
    {
        private readonly IF152ReportService _reportService;

        public F152ReportController(IF152ReportService reportService)
        {
            _reportService = reportService;
        }

        // Get all transactions (with their categories)
        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<F152ReportTransactionDto>>> GetAllTransactions()
        {
            var transactions = await _reportService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        // Get a transaction by ID (with its categories)
        [HttpGet("transactions/{id}")]
        public async Task<ActionResult<F152ReportTransactionDto>> GetTransactionById(int id)
        {
            var transaction = await _reportService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        // Create a new transaction
        [HttpPost("transactions")]
        public async Task<ActionResult<F152ReportTransactionDto>> CreateTransaction([FromBody] F152ReportTransactionDto transactionDto)
        {
            if (transactionDto == null)
                return BadRequest("Invalid data.");

            var createdTransaction = await _reportService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransactionById), new { id = createdTransaction.Id }, createdTransaction);
        }

        // Update an existing transaction
        [HttpPut("transactions/{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] F152ReportTransactionDto transactionDto)
        {
            var result = await _reportService.UpdateTransactionAsync(id, transactionDto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Delete a transaction
        [HttpDelete("transactions/{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var result = await _reportService.DeleteTransactionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Get categories by transaction ID
        [HttpGet("transactions/{transactionId}/categories")]
        public async Task<ActionResult<IEnumerable<F152ReportCategoryDto>>> GetCategoriesByTransactionId(int transactionId)
        {
            var categories = await _reportService.GetCategoriesByTransactionIdAsync(transactionId);
            return Ok(categories);
        }

        // Create a new category under a specific transaction
        [HttpPost("transactions/{transactionId}/categories")]
        public async Task<ActionResult<F152ReportCategoryDto>> CreateCategory(int transactionId, [FromBody] F152ReportCategoryDto categoryDto)
        {
            var createdCategory = await _reportService.CreateCategoryAsync(transactionId, categoryDto);
            if (createdCategory == null)
                return NotFound();

            return CreatedAtAction(nameof(GetCategoriesByTransactionId), new { transactionId = transactionId }, createdCategory);
        }

        // Update an existing category
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] F152ReportCategoryDto categoryDto)
        {
            var result = await _reportService.UpdateCategoryAsync(id, categoryDto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Delete a category
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _reportService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
