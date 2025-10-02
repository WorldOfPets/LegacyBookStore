using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LegacyBookStore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LegacyBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration _configuration) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
#if DEBUG
            if (request.username == "bogdan")
            {
                var token = GenerateJwtToken(request.username);
                // Установка куки
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                    SameSite = SameSiteMode.Strict,
                    Secure = false
                };
                Response.Cookies.Append("access_token", token, cookieOptions);

                return Ok(new { message = "Login successful" });
            }
#endif
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("checkToken")]
        public IActionResult CheckToken()
        {
            return Ok();
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logout successful" });
        }
        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
