using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Services.InventoryService;

namespace Pacifica.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)] // Exclude this controller from Swagger UI
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Endpoint to create Weekly Inventory
        [HttpPost("weekly")]
        public async Task<IActionResult> CreateWeeklyInventory([FromBody] CreateWeeklyInventoryDTO inventoryDto)
        {
            var response = await _inventoryService.CreateWeeklyInventoryAsync(inventoryDto);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<ResponseWeeklyInventoryDTO>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            // Returning the created inventory with a 201 status code
            return CreatedAtAction(nameof(GetWeeklyInventoryById), new { id = response.Data!.Id }, response);
        }

        // Get Weekly Inventory by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeeklyInventoryById(int id)
        {
            var response = await _inventoryService.GetWeeklyInventoryByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(new ApiResponse<ResponseWeeklyInventoryDTO>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            return Ok(response);
        }

        // Get Filtered Weekly Inventories
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredWeeklyInventoriesAsync([FromQuery] FilterWeeklyInventoryParams filterParams)
        {
            var response = await _inventoryService.GetFilteredWeeklyInventoriesAsync(filterParams);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<IEnumerable<ResponseWeeklyInventoryDTO>>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            return Ok(response);
        }
   
   
        [HttpGet("view")]
        public async Task<IActionResult> GetViewInventories([FromQuery] ViewInventoryParams filterParams)
        {
            var response = await _inventoryService.GetViewInventoriesAsync(filterParams);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<ResponseViewInventoryDTO>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            return Ok(response);
        }
   
   
   }
}
