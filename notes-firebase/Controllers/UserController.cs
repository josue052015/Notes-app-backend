using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notes_firebase.DTOs;
using notes_firebase.Models;
using notes_firebase.Services;

namespace notes_firebase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService) 
        {
            this._userService = userService;
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<UserDTO> GetUser(string id)
        {
            return await _userService.GetUserById(id);
        }
        [HttpPost]
        public async Task<UserDTO> AddUser([FromBody] User user)
        {
            return await _userService.AddUser(user);
        }
        [HttpPut]
        [Authorize]
        public async Task<UserDTO> EditUser([FromBody] UserDTO user)
        {
            return await _userService.EditUser(user);
        }
        [HttpDelete]
        [Authorize]
        public async Task<string> DeleteUser(string id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
