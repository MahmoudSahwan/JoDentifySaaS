using JoDentify.Application.DTOs.Auth;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JoDentify.API.Controllers
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


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);

            
            if (string.IsNullOrEmpty(result.Token))
            {
                return BadRequest(result.Message); 
            }

            return Ok(result); 
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

           
            if (string.IsNullOrEmpty(result.Token))
            {
                return BadRequest(result.Message); 
            }

            return Ok(result);
        }
    }
}

