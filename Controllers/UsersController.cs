using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // user register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto userDto)
        {
            return await _userService.RegisterAsync(userDto);
        }

        // user login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            return await _userService.LoginAsync(loginDto);
        }

        // user ID list
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            return await _userService.GetUserAsync(id);
        }

        // user list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userService.GetUsersAsync();
        }

        // user update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            return await _userService.UpdateUserAsync(id, userDto);
        }

        // user delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        //user serach
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<User>>> Search([FromBody] SearchUserDto searchUserDto)
        {
            return await _userService.SearchUsersAsync(searchUserDto);
        }
    }
}
