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
        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryDTO inventoryDto)
        {
            var response = await _inventoryService.CreateInventoryAsync(inventoryDto);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<ResponseInventoryDTO>
                {
                    Success = false,
                    Message = response.Message,
                    Data = null
                });
            }

            // Returning the created inventory with a 201 status code
            return CreatedAtAction(nameof(GetInventoryById), new { id = response.Data!.Id }, response);
        }

        // Get Weekly Inventory by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var response = await _inventoryService.GetInventoryByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(new ApiResponse<ResponseInventoryDTO>
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
        public async Task<IActionResult> GetFilteredInventoriesAsync([FromQuery] FilterInventoryParams filterParams)
        {
            var response = await _inventoryService.GetFilteredInventoriesAsync(filterParams);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<IEnumerable<ResponseInventoryDTO>>
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

        [HttpGet("search-branchproduct")]
        public async Task<IActionResult> GetFilteredBranchProduct([FromQuery] WI_BranchProductSearchParams filterParams)
        {
            var response = await _inventoryService.GetFilteredBranchProductAsync(filterParams);

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

