using Pacifica.API.Dtos.UserManagement;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.EmployeManagement;

namespace Pacifica.API.Services.EmployeeManagementService
{
    public class PositionService : IPositionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PositionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all positions
        public async Task<ApiResponse<IEnumerable<Position>>> GetAllAsync()
        {
            var positions = await _context.Positions
                // .Include(p => p.Employees) // Optional: include Employees if needed
                .ToListAsync();

            return new ApiResponse<IEnumerable<Position>>
            {
                Success = true,
                Message = "Positions retrieved successfully.",
                Data = positions,
                TotalCount = positions.Count
            };
        }

        // Get a position by Id
        public async Task<ApiResponse<Position?>> GetByIdAsync(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                return new ApiResponse<Position?>
                {
                    Success = false,
                    Message = "Position not found.",
                    Data = null
                };
            }

            return new ApiResponse<Position?>
            {
                Success = true,
                Message = "Position retrieved successfully.",
                Data = position
            };
        }

        // Create a new position
        public async Task<ApiResponse<Position>> CreateAsync(PositionDto positionDto)
        {
            var position = _mapper.Map<Position>(positionDto);
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return new ApiResponse<Position>
            {
                Success = true,
                Message = "Position created successfully.",
                Data = position
            };
        }

        // Update an existing position
        public async Task<ApiResponse<Position?>> UpdateAsync(int id, PositionDto positionDto)
        {
            var existingPosition = await _context.Positions.FindAsync(id);
            if (existingPosition == null)
            {
                return new ApiResponse<Position?>
                {
                    Success = false,
                    Message = "Position not found.",
                    Data = null
                };
            }

            existingPosition.Name = positionDto.Name;
            existingPosition.Remarks = positionDto.Remarks;

            await _context.SaveChangesAsync();

            return new ApiResponse<Position?>
            {
                Success = true,
                Message = "Position updated successfully.",
                Data = existingPosition
            };
        }

        // Delete a position by Id
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Position not found.",
                    Data = false
                };
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Position deleted successfully.",
                Data = true
            };
        }
    }
}
