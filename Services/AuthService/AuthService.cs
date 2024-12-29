
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Services.TokenService;

namespace Pacifica.API.Services.AuthService
{

    public class AuthService : IAuthService
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthService(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IMapper mapper, ITokenService tokenService, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;

        }

        public async Task<ApiResponse<object>> LoginAsync(LoginDto loginDto)
        {
            var employee = await _userManager.Users
                .Include(e => e.EmployeeBranches)  // Eagerly load the EmployeeBranches collection
                .FirstOrDefaultAsync(e => e.UserName == loginDto.Username);

            if (employee == null)
            {
                return new ApiResponse<object> { Success = false, Message = "Invalid login attempt" };
            }

            var result = await _signInManager.PasswordSignInAsync(employee, loginDto.Password!, false, false);

            if (result.Succeeded)
            {
                var token = await _tokenService.GenerateToken(employee);
                var branches = employee.EmployeeBranches?.ToList() ?? new List<EmployeeBranch>();  // Fallback to an empty list

                // Ensure that each branch has its Branch object populated
                foreach (var employeeBranch in branches)
                {
                    // Fetch the branch details for the given BranchId
                    employeeBranch.Branch = await _context.Branches
                        .FirstOrDefaultAsync(b => b.Id == employeeBranch.BranchId); // Assuming _branchRepository is available
                }

                var responseData = new
                {
                    employee.Email,
                    employee.EmployeeId,
                    userId = employee.Id,
                    fullname = employee.FirstName + " " + employee.LastName,
                    employee.Position,
                    Roles = await _userManager.GetRolesAsync(employee),
                    Branches = branches.Select(eb => new
                    {
                        eb.BranchId,
                        BranchName = eb.Branch?.BranchName,  // Assuming each branch has a Name property
                        BranchLocation = eb.Branch?.BranchLocation  // Assuming each branch has a Location property
                    }).ToList(),
                    Token = token
                };

                return new ApiResponse<object> { Success = true, Message = "Login successful", Data = responseData };
            }

            return result.IsLockedOut
                ? new ApiResponse<object> { Success = false, Message = "Your account is locked. Please try again later." }
                : result.IsNotAllowed
                ? new ApiResponse<object> { Success = false, Message = "Your account is not allowed to sign in." }
                : new ApiResponse<object> { Success = false, Message = "Invalid login attempt" };
        }



    }
}

