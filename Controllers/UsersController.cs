using ECommerceAPI.Models.DTOs;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        // user register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _context.Users.AnyAsync(u => u.Mail == userDto.Mail))
            {
                return BadRequest("Bu e-posta adresi zaten kullanılıyor.");
            }
            var user = new User
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Mail = userDto.Mail,
                Phone = userDto.Phone,
                Role = "",
                Password = userDto.Password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { message = "Kullanıcı başarıyla kaydedildi." });
        }

        // user login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Mail == loginDto.Mail);
            if (user == null || user.Password != loginDto.Password)
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }
            return Ok(new { message = "Giriş başarılı." });
        }

        // user ID list
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        // user list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || !users.Any())
            {
                return NotFound(new { message = "Hiçbir kullanıcı bulunamadı." });
            }
            return Ok(users);
        }
        // user update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }
            
            var existingMailUser = await _context.Users.Where(u => u.Mail == userDto.Mail && u.Id != id).FirstOrDefaultAsync();
            if (existingMailUser != null)
            {
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor." });
            }
            existingUser.Name = userDto.Name;
            existingUser.Surname = userDto.Surname;
            existingUser.Mail = userDto.Mail;
            existingUser.Phone = userDto.Phone;
            existingUser.Role = userDto.Role;
            existingUser.Password = userDto.Password;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Kullanıcı başarıyla güncellendi." });
        }
        // user delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Kullanıcı başarıyla silindi." });
        }
    }
}
