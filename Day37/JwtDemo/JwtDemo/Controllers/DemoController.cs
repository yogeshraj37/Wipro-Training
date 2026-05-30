using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtDemo.Controllers
{
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DemoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("api/login")]
        public IActionResult Login()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtKey"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("name", "Parth")
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            string jwtToken =
                new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(jwtToken);
        }

        [Authorize]
        [HttpGet("data")]
        public IActionResult GetData()
        {
            return Ok(
                "This is protected data. Token verified!");
        }
    }
}