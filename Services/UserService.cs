﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;

namespace PCM.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<object> _passwordHasher;

        public UserService(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<User> RegisterUserAsync(string email, string password)
        {
            var passwordHash = HashPassword(password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> AuthenticateUserAsync(string Email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            user.IsActive = true;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return user;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }


        public bool VerifyPassword(string password, string hash)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hash, password);
            return result == PasswordVerificationResult.Success;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }



        public async Task BlockUsersAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = true;
            }
            await _context.SaveChangesAsync();
        }


        public async Task UnblockUsersAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsersAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task MakeAdminsAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.Role = "Admin";
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAdminsAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.Role = UserRole.User;
            }
            await _context.SaveChangesAsync();
        }

        public async Task LogoutAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.IsActive = false;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            // Retrieve the user by ID asynchronously
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IsAdminAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            return user.Role == UserRole.Admin && !user.IsBlocked;
        }
    }
}
