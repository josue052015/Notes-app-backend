using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using notes_firebase.Models;
using notes_firebase.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace notes_firebase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly AuthService _authService;

        public AuthController(IConfiguration configuration, AuthService authService) 
        {
            this.configuration = configuration;
            this._authService = authService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] UserLogin userLogin)
        {
            var user = await _authService.FindUser(userLogin.Email, userLogin.Password);
            if (user is not null)
            {
                var issuer = configuration["JWT:Issuer"];
                var audience = configuration["JWT:Audience"];
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                    );
                var subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userLogin.Email),
                new Claim(JwtRegisteredClaimNames.Email, userLogin.Email),
                new Claim(ClaimTypes.NameIdentifier, user),
              });
                var expires = DateTime.UtcNow.AddDays(1);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = expires,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jtwToken = tokenHandler.WriteToken(token);
                return Ok(jtwToken);
            }
          return Unauthorized();
        }
    }
}
