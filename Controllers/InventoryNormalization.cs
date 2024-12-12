using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Models.Inventory;

using Pacifica.API.Services.InventoryNormalizationService;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryNormalizationController : ControllerBase
    {
        private readonly IInventoryNormalizationService _service;

        public InventoryNormalizationController(IInventoryNormalizationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var normalizations = await _service.GetAllAsync();
            return Ok(normalizations);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var normalization = await _service.GetByIdAsync(id);
            if (normalization == null)
                return NotFound();
            
            return Ok(normalization);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryNormalizationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdNormalization = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdNormalization.Id }, createdNormalization);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryNormalizationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedNormalization = await _service.UpdateAsync(id, dto);
            if (updatedNormalization == null)
                return NotFound();

            return Ok(updatedNormalization);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
