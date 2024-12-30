
using System.Linq.Expressions;
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

        public async Task<BeginningInventoryDto> CreateAsync(CreateBeginningInventoryDto model)
        {
            var entity = _mapper.Map<BeginningInventory>(model);

            _context.BeginningInventories.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeginningInventoryDto>(entity);
        }

        public async Task<BeginningInventoryDto> GetByIdAsync(int id)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return null!;
            }

            return _mapper.Map<BeginningInventoryDto>(entity);
        }

        public async Task<IEnumerable<BeginningInventoryDto>> GetAllAsync()
        {
            var entities = await _context.BeginningInventories.ToListAsync();
            return _mapper.Map<IEnumerable<BeginningInventoryDto>>(entities);
        }

        public async Task<BeginningInventoryDto> UpdateAsync(int id, UpdateBeginningInventoryDto model)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return null!;
            }

            _mapper.Map(model, entity); // Map the updated DTO to the entity

            _context.BeginningInventories.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeginningInventoryDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BeginningInventories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.BeginningInventories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApiResponse<List<BeginningInventoryDto>>> GetAllAsync(int page, int size, string sortField, int sortOrder)
        {
            var query = _context.BeginningInventories.AsQueryable();

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
                Message = "Data retrieved successfully",
                Data = result,
                TotalCount = totalRecords
            };
        }

    }
}

