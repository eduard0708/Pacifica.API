using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Services.BeginningInventoryService;
using System.Threading.Tasks;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeginningInventoryController : ControllerBase
    {
        private readonly IBeginningInventoryService _beginningInventoryService;

        public BeginningInventoryController(IBeginningInventoryService beginningInventoryService)
        {
            _beginningInventoryService = beginningInventoryService;
        }

        // POST: api/BeginningInventory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBeginningInventoryDto model)
        {
            var response = await _beginningInventoryService.CreateAsync(model);
            if (response.Success)
            {
                return Ok(response); // Successful creation
            }

            // Return a 400 or 409 status code based on the situation
            if (response.Message.Contains("already exists"))
            {
                // If the inventory for the year already exists, return a 409 Conflict
                return Conflict(response);
            }

            return BadRequest(response); // Return error response
        }

        // GET: api/BeginningInventory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _beginningInventoryService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response); // Return success with the data
            }
            return NotFound(response); // Return not found response if the record does not exist
        }

        // GET: api/BeginningInventory
        [HttpGet]
        public async Task<IActionResult> GetAllLazy([FromQuery] int page = 0, [FromQuery] int size = 10,
                                                 [FromQuery] string sortField = "Id", [FromQuery] int sortOrder = 1)
        {
            var response = await _beginningInventoryService.GetAllLazyAsync(page, size, sortField, sortOrder);
            return Ok(response); // Return paginated data with success
        }

        // GET: api/BeginningInventory/branch/{branchId}
        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetByBranchId(int branchId)
        {
            var response = await _beginningInventoryService.GetByBranchIdAsync(branchId);
            if (response.Success)
            {
                return Ok(response); // Return success with the data
            }
            return NotFound(response); // Return not found response if no records found for the branch
        }

        // PUT: api/BeginningInventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBeginningInventoryDto model)
        {
            var response = await _beginningInventoryService.UpdateAsync(id, model);
            if (response.Success)
            {
                return Ok(response); // Return success with updated data
            }
            return NotFound(response); // Return not found response if the record does not exist
        }

        // DELETE: api/BeginningInventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _beginningInventoryService.DeleteAsync(id);
            if (response.Success)
            {
                return Ok(response); // Return success response for deletion
            }
            return NotFound(response); // Return not found response if the record does not exist
        }
    }
}
