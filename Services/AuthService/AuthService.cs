using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PacificaAPI.Dtos.Admin;

namespace PacificaAPI.Services.AuthService
{

    public class AuthService : IAuthService
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;

        public AuthService(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginDto loginDto)
        {
            // Check if the employee exists with the provided email
            var employee = await _userManager.FindByNameAsync(loginDto.Username);
            if (employee == null)
            {
                return new ApiResponse<string> { Success = false, Message = "Invalid login attempt" };
            }

            // Attempt to sign in with the provided password
            var result = await _signInManager.PasswordSignInAsync(employee, loginDto.Password, false, false);

            // If the sign-in was successful, return a success response with a placeholder for the token
            if (result.Succeeded)
            {
                return new ApiResponse<string> { Success = true, Message = "Login successful", Data = "TokenPlaceholder" };
            }

            // If the sign-in failed, handle various failure scenarios
            if (result.IsLockedOut)
            {
                return new ApiResponse<string> { Success = false, Message = "Your account is locked. Please try again later." };
            }

            if (result.IsNotAllowed)
            {
                return new ApiResponse<string> { Success = false, Message = "Your account is not allowed to sign in." };
            }

            // Default invalid login attempt message for incorrect username/password
            return new ApiResponse<string> { Success = false, Message = "Invalid login attempt" };
        }


        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto)
        {
            var employee = _mapper.Map<Employee>(registerDto);
            // Set the Username field explicitly to the EmployeeId
            employee.UserName = registerDto.EmployeeId;

            var result = await _userManager.CreateAsync(employee, registerDto.Password);


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
