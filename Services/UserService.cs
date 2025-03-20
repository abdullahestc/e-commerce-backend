using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using ECommerceAPI.Common;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        Task<ActionResult<User>> RegisterAsync(UserDto userDto);
        Task<ActionResult> LoginAsync(LoginDto loginDto);
        Task<ActionResult<User>> GetUserAsync(int id);
        Task<ActionResult<IEnumerable<User>>> GetUsersAsync();
        Task<IActionResult> UpdateUserAsync(int id, UserDto userDto);
        Task<IActionResult> DeleteUserAsync(int id);
        Task<ActionResult<IEnumerable<User>>> SearchUsersAsync(SearchUserDto searchUserDto);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
            MappingConfig.Configure();
        }

        //user register
        public async Task<ActionResult<User>> RegisterAsync(UserDto userDto)
        {
            var isMailExists = _context.Users.Any(u => u.Mail == userDto.Mail);
            if (isMailExists)
            {
                return new BadRequestObjectResult(new { message = Messages.UsedMail });
            }
            
            var user = userDto.Adapt<User>();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = Messages.AddUser });
        }

        //user login
        public async Task<ActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Mail == loginDto.Mail);
            if (user == null || user.Password != loginDto.Password)
            {
                return new UnauthorizedObjectResult(new { message = Messages.IncorrectEntry });
            }

            return new OkObjectResult(new { message = Messages.CorrectEntry });
        }

        //user Id list
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            return user;
        }

        //user list
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || !users.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(users);
        }

        //user update
        public async Task<IActionResult> UpdateUserAsync(int id, UserDto userDto)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return new NotFoundResult();
            }

            var isMailUsed = _context.Users.Any(u => u.Mail == userDto.Mail && u.Id != id);
            if (isMailUsed)
            {
                return new BadRequestObjectResult(new { message = Messages.UsedMail });
            }

            existingUser = userDto.Adapt(existingUser);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = Messages.UpdateUser });
        }

        //user delete
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = Messages.DeleteUser });
        }

        //user search
        public async Task<ActionResult<IEnumerable<User>>> SearchUsersAsync(SearchUserDto searchUserDto)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchUserDto.Name))
            {
                query = query.Where(u => u.Name.ToLower().Contains(searchUserDto.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchUserDto.Surname))
            {
                query = query.Where(u => u.Surname.ToLower().Contains(searchUserDto.Surname.ToLower()));
            }
            
            if (!string.IsNullOrEmpty(searchUserDto.Role))
            {
                query = query.Where(u => u.Role.ToLower().Contains(searchUserDto.Role.ToLower()));
            }

            var users = await query.ToListAsync();
            if (users == null || !users.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(users);
        }
    }
}

