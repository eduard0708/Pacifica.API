using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Models.Inventory;

namespace Pacifica.API.Services.BeginningInventoryService
{
    public class BeginningInventoryService : IBeginningInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BeginningInventoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Create a new BeginningInventory
        public async Task<ApiResponse<BeginningInventoryDto>> CreateAsync(CreateBeginningInventoryDto model)
        {
            // Extract the year from the BeginningInventoryDate
            int year = model.BeginningInventoryDate.Year;

            // Check if there's already a record with the same year for the same branch
            bool yearExists = await _context.BeginningInventories
                .AnyAsync(b => b.BeginningInventoryDate.Year == year && b.BranchId == model.BranchId);

            if (yearExists)
            {
                // Return a response indicating that the inventory for the given year already exists
                return new ApiResponse<BeginningInventoryDto>
                {
                    Success = false,
                    Message = $"A beginning inventory for the year {year} already exists for this branch.",
                    Data = null
                };
            }

            // Map the DTO to the entity
            var entity = _mapper.Map<BeginningInventory>(model);

            // Add the entity to the context
            _context.BeginningInventories.Add(entity);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the mapped DTO in a success response
            var result = _mapper.Map<BeginningInventoryDto>(entity);
            return new ApiResponse<BeginningInventoryDto>
            {
                Success = true,
                Message = "Beginning inventory created successfully.",
                Data = result
            };
        }


        // Get a BeginningInventory by its Id
        public async Task<ApiResponse<BeginningInventoryDto>> GetByIdAsync(int id)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse<BeginningInventoryDto>
                {
                    Success = false,
                    Message = "Beginning inventory not found.",
                    Data = null
                };
            }

            return new ApiResponse<BeginningInventoryDto>
            {
                Success = true,
                Message = "Beginning inventory retrieved successfully.",
                Data = _mapper.Map<BeginningInventoryDto>(entity)
            };
        }

        // Get all BeginningInventories with pagination and sorting
        public async Task<ApiResponse<List<BeginningInventoryDto>>> GetAllLazyAsync(int page, int size, string sortField, int sortOrder)
        {
            var query = _context.BeginningInventories.Include(b => b.Branch).AsQueryable();

            // Sorting Logic
            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortOrder == 1)
                    query = query.OrderBy(sortField); // Ascending order
                else
                    query = query.OrderByDescending(sortField); // Descending order
            }

            // Apply Pagination
            var totalRecords = await query.CountAsync();
            var inventories = await query.Skip(page * size).Take(size).ToListAsync();

            var result = _mapper.Map<List<BeginningInventoryDto>>(inventories);

            return new ApiResponse<List<BeginningInventoryDto>>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = result,
                TotalCount = totalRecords
            };
        }

        // Get BeginningInventories by BranchId
        public async Task<ApiResponse<List<BeginningInventoryDto>>> GetByBranchIdAsync(int branchId)
        {
            var entities = await _context.BeginningInventories
                .Where(x => x.BranchId == branchId)
                .ToListAsync();

            if (entities == null || !entities.Any())
            {
                return new ApiResponse<List<BeginningInventoryDto>>
                {
                    Success = false,
                    Message = $"No beginning inventories found for BranchId {branchId}.",
                    Data = new List<BeginningInventoryDto>()
                };
            }

            var result = _mapper.Map<List<BeginningInventoryDto>>(entities);

            return new ApiResponse<List<BeginningInventoryDto>>
            {
                Success = true,
                Message = "Beginning inventories retrieved successfully.",
                Data = result
            };
        }

        // Update an existing BeginningInventory
        public async Task<ApiResponse<BeginningInventoryDto>> UpdateAsync(int id, UpdateBeginningInventoryDto model)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse<BeginningInventoryDto>
                {
                    Success = false,
                    Message = "Beginning inventory not found.",
                    Data = null
                };
            }

            _mapper.Map(model, entity); // Map the updated DTO to the entity

            _context.BeginningInventories.Update(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<BeginningInventoryDto>
            {
                Success = true,
                Message = "Beginning inventory updated successfully.",
                Data = _mapper.Map<BeginningInventoryDto>(entity)
            };
        }

        // Delete a BeginningInventory by its Id
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Beginning inventory not found.",
                    Data = false
                };
            }

            _context.BeginningInventories.Remove(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Beginning inventory deleted successfully.",
                Data = true
            };
        }
    }
}
