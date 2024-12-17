using Pacifica.API.Dtos.UserManagement;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.EmployeManagement;

namespace Pacifica.API.Services.EmployeeManagementService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all departments
        public async Task<ApiResponse<IEnumerable<Department>>> GetAllAsync()
        {
            var departments = await _context.Departments
                // .Include(d => d.Employees) // Optional: include Employees if needed
                .ToListAsync();

            return new ApiResponse<IEnumerable<Department>>
            {
                Success = true,
                Message = "Departments retrieved successfully.",
                Data = departments,
                TotalCount = departments.Count
            };
        }

        // Get a department by Id
        public async Task<ApiResponse<Department?>> GetByIdAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return new ApiResponse<Department?>
                {
                    Success = false,
                    Message = "Department not found.",
                    Data = null
                };
            }

            return new ApiResponse<Department?>
            {
                Success = true,
                Message = "Department retrieved successfully.",
                Data = department
            };
        }

        // Create a new department
        public async Task<ApiResponse<Department>> CreateAsync(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new ApiResponse<Department>
            {
                Success = true,
                Message = "Department created successfully.",
                Data = department
            };
        }

        // Update an existing department
        public async Task<ApiResponse<Department?>> UpdateAsync(int id, DepartmentDto departmentDto)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return new ApiResponse<Department?>
                {
                    Success = false,
                    Message = "Department not found.",
                    Data = null
                };
            }

            existingDepartment.Name = departmentDto.Name;
            existingDepartment.Remarks = departmentDto.Remarks;

            await _context.SaveChangesAsync();

            return new ApiResponse<Department?>
            {
                Success = true,
                Message = "Department updated successfully.",
                Data = existingDepartment
            };
        }

        // Delete a department by Id
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Department not found.",
                    Data = false
                };
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Department deleted successfully.",
                Data = true
            };
        }
    }
}
