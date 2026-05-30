using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleJWTDemo.Controllers
{
    public class DemoController : Controller
    {
        //here we can add some endpoints to test the JWT authentication
        //login api to generate token
        //secure api to test token validation

        private readonly IConfiguration _configuration;

        public DemoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("api/login")]
        public IActionResult Login()
        {
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwtkey"]));

            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("name", "Parth"),
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            String jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwtToken);

        }
        //secure api endpoint that requires authentication
        //this Api will be accessible only if a valid JWT token is provided in the request header
        [Authorize]
        [HttpGet("data")]
        public IActionResult GetData()
        {
            return Ok(new { message = "This is a secure data endpoint. You are authenticated!" });
        }


    }
}