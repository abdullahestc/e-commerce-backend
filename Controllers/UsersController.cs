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
        
        //user register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await _userService.RegisterAsync(userDto);
            
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        //user login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
        
            if (!result.Success)
                return Unauthorized(result.Message);
            
            return Ok(result.Message);
        }

        //user Id list
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserAsync(id);
        
            if (result.NotFound)
                return NotFound(result.Message);
            
            return Ok(result.Data);
        }

        //user list
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
        
            if (result.NotFound)
                return NotFound(result.Message);
            
            return Ok(result.Data);
        }

        //user update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(id, userDto);
        
            if (result.NotFound)
                return NotFound(result.Message);
            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        //user delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
        
            if (result.NotFound)
                return NotFound(result.Message);
            
            return Ok(result.Message);
        }

        //user search
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchUserDto searchUserDto)
        {
            var result = await _userService.SearchUsersAsync(searchUserDto);
        
            if (result.NotFound)
                return NotFound(result.Message);
            
            return Ok(result.Data);
        }
    }
}
