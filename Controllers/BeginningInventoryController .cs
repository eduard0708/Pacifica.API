using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Services.BeginningInventoryService;

namespace Pacifica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeginningInventoryController : ControllerBase
    {
        private readonly IBeginningInventoryService _service;

        public BeginningInventoryController(IBeginningInventoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBeginningInventoryDto model)
        {
            var result = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 0, [FromQuery] int size = 10, [FromQuery] string sortField = "", [FromQuery] int sortOrder = 1)
        {
            var result = await _service.GetAllAsync(page, size, sortField, sortOrder);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     var result = await _service.GetAllAsync();
        //     return Ok(result);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBeginningInventoryDto model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
