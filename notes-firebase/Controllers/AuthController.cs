using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors]
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
                return Ok(_authService.GenerateToken(userLogin, user));
            }
          return Unauthorized();
        }
    }
}
