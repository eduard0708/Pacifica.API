using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PacificaAPI.Dtos.Admin;
using PacificaAPI.Services.AuthService;

namespace PacificaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(RegisterDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            return Ok(response);
        }
    }
}