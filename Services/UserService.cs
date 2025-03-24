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
        Task<ServiceResult<User>> RegisterAsync(UserDto userDto);
        Task<ServiceResult> LoginAsync(LoginDto loginDto);
        Task<ServiceResult<User>> GetUserAsync(int id);
        Task<ServiceResult<IEnumerable<User>>> GetUsersAsync();
        Task<ServiceResult<UserDto>> UpdateUserAsync(int id, UserDto userDto);
        Task<ServiceResult> DeleteUserAsync(int id);
        Task<ServiceResult<IEnumerable<User>>> SearchUsersAsync(SearchUserDto searchUserDto);
        Task<ServiceResult<int>> GetUserCountAsync(); 
        
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
        public async Task<ServiceResult<User>> RegisterAsync(UserDto userDto)
        {
            var isMailExists = _context.Users.Any(u => u.Mail == userDto.Mail);
            if (isMailExists)
            {
                return ServiceResult<User>.ErrorResult(Messages.UsedMail);
            }
            
            var user = userDto.Adapt<User>();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return ServiceResult<User>.SuccessResult(user, Messages.AddUser);
        }

        //user login
        public async Task<ServiceResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Mail == loginDto.Mail);
            if (user == null || user.Password != loginDto.Password)
            {
                return ServiceResult.ErrorResult(Messages.IncorrectEntry);
            }

            return ServiceResult.SuccessResult(Messages.CorrectEntry);
        }

        //user Id list
        public async Task<ServiceResult<User>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ServiceResult<User>.NotFoundResult(Messages.UserNotFound);
            }

            return ServiceResult<User>.SuccessResult(user);
        }

        //user list
        public async Task<ServiceResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || !users.Any())
            {
                return ServiceResult<IEnumerable<User>>.NotFoundResult(Messages.UserNotFound);
            }

            return ServiceResult<IEnumerable<User>>.SuccessResult(users);
        }

        //user update
        public async Task<ServiceResult<UserDto>> UpdateUserAsync(int id, UserDto userDto)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return ServiceResult<UserDto>.NotFoundResult(Messages.UserNotFound);
            }

            var isMailUsed = _context.Users.Any(u => u.Mail == userDto.Mail && u.Id != id);
            if (isMailUsed)
            {
                return ServiceResult<UserDto>.ErrorResult(Messages.UsedMail);
            }

            existingUser = userDto.Adapt(existingUser);
            await _context.SaveChangesAsync();

            return ServiceResult<UserDto>.SuccessResult(userDto, Messages.UpdateUser);
        }

        //user delete
        public async Task<ServiceResult> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ServiceResult.NotFoundResult(Messages.UserNotFound);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ServiceResult.SuccessResult(Messages.DeleteUser);
        }

        //user search
        public async Task<ServiceResult<IEnumerable<User>>> SearchUsersAsync(SearchUserDto searchUserDto)
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
                return ServiceResult<IEnumerable<User>>.NotFoundResult(Messages.UserNotFound);
            }

            return ServiceResult<IEnumerable<User>>.SuccessResult(users);
        }

        //user count
        public async Task<ServiceResult<int>> GetUserCountAsync()
        {
            try
            {
                var count = await _context.Users.CountAsync();
                return ServiceResult<int>.SuccessResult(count);
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.ErrorResult($"{ex.Message}");
            }
        }
    }
}

