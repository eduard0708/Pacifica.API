using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Services.InventoryNormalizationService;
using Pacifica.API.Models.Inventory;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryNormalizationController : ControllerBase
    {
        private readonly IInventoryNormalizationService _inventoryNormalizationService;

        // Constructor injection of the service
        public InventoryNormalizationController(IInventoryNormalizationService inventoryNormalizationService)
        {
            _inventoryNormalizationService = inventoryNormalizationService;
        }

        // GET: api/InventoryNormalization
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryNormalization>>> GetAll()
        {
            var normalizations = await _inventoryNormalizationService.GetAllAsync();
            return Ok(normalizations);
        }

        // GET: api/InventoryNormalization/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryNormalization>> GetById(int id)
        {
            var normalization = await _inventoryNormalizationService.GetByIdAsync(id);
            if (normalization == null)
            {
                return NotFound();
            }
            return Ok(normalization);
        }

        // POST: api/InventoryNormalization
        [HttpPost("create")]
        public async Task<ActionResult<InventoryNormalization>> Create(CreateInventoryNormalizationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var normalization = await _inventoryNormalizationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = normalization.Id }, normalization);
        }

        // PUT: api/InventoryNormalization/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryNormalization>> Update(int id, InventoryNormalizationDto dto)
        {
            var updatedNormalization = await _inventoryNormalizationService.UpdateAsync(id, dto);
            if (updatedNormalization == null)
            {
                return NotFound();
            }
            return Ok(updatedNormalization);
        }

        // DELETE: api/InventoryNormalization/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _inventoryNormalizationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET api/inventory/filteredBranchProducts
        [HttpGet("normalize")]
        public async Task<IActionResult> GetFilteredBranchProductWithDiscrepancyAsync([FromQuery] InventoryNormalizeParams filterParams)
        {
            // Validate filter parameters
            if (filterParams == null)
            {
                return BadRequest("Filter parameters are required.");
            }

            // Call the service method to get filtered products with discrepancies
            var response = await _inventoryNormalizationService.GetFilteredBranchProductWithDiscrepancyAsync(filterParams);

            // Check the response status
            if (response.Success)
            {
                return Ok(response); // Return the data if successful
            }

            // Return error response if no products are found or there's an error
            return NotFound(new { message = response.Message });
        }


    }
}
