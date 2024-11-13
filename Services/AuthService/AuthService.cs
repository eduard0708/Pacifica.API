using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Helper;
using Pacifica.API.Services.TokenService;

namespace Pacifica.API.Services.AuthService
{

    public class AuthService : IAuthService
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;


        public AuthService(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IMapper mapper, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;

        }

        public async Task<ApiResponse<object>> LoginAsync(LoginDto loginDto)
        {
            var employee = await _userManager.FindByNameAsync(loginDto.Username!);
            if (employee == null)
            {
                return new ApiResponse<object> { Success = false, Message = "Invalid login attempt" };
            }

            var result = await _signInManager.PasswordSignInAsync(employee, loginDto.Password!, false, false);

            if (result.Succeeded)
            {
                var token = await _tokenService.GenerateToken(employee);

                var responseData = new
                {
                    employee.Email,
                    employee.EmployeeId,
                    employee.Id,
                    // employee.EmployeeProfile.FirstName,
                    // employee.EmployeeProfile.LastName,
                    Roles = await _userManager.GetRolesAsync(employee),
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
        
        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto)
        {
            var employee = _mapper.Map<Employee>(registerDto);
            // Set the Username field explicitly to the EmployeeId
            employee.UserName = registerDto.EmployeeId;

            var result = await _userManager.CreateAsync(employee, registerDto.Password!);


            if (result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Employee registered successfully",
                    Data = "TokenPlaceholder"
                };
            }
            else
            {
                // Collect error messages from IdentityResult
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));

                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error registering employee: {errorMessage}"
                };
            }
        }

    }

}
