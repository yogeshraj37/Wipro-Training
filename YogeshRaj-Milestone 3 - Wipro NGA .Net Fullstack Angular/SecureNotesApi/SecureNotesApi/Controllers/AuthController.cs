using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureNotesApi.Models;
using SecureNotesApi.Services;

namespace SecureNotesApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterDto model)
        {
            var userExists =
                await _userManager.FindByNameAsync(
                    model.Username);

            if (userExists != null)
            {
                return BadRequest(
                    new
                    {
                        message = "Username already exists"
                    });
            }

            var user = new ApplicationUser
            {
                UserName = model.Username
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message =
                "User registered successfully. Please log in."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto model)
        {
            var user =
                await _userManager.FindByNameAsync(
                    model.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            var validPassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    model.Password);

            if (!validPassword)
            {
                return Unauthorized();
            }

            var token =
                _jwtService.GenerateToken(user);

            return Ok(new
            {
                token,
                expires_in = 3600,
                user = new
                {
                    username = user.UserName
                }
            });
        }
    }
}