using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OrderSystem.Contracts;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private string _key = "b82fe40b63ac45f99c8a1f4f9e3cb16a52ea9b2ee38f4059";
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Örnek kullanıcı doğrulaması
        if (request.Username == "admin" && request.Password == "1234")
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenStr });
        }

        return Unauthorized();
    }
}
