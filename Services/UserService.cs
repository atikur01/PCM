using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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

        public async Task<User> RegisterUserAsync(string email, string password, string Name )
        {
            var isexist = await GetUserByEmailAsync(email);
            
            if (isexist != null)
            {
                return null;
            }

            var passwordHash = HashPassword(password);
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = Name,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User,
                CreatedAt = DateTime.Now    
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> AuthenticateUserAsync(string Email, string password)
        {
            var user = await GetUserByEmailAsync(Email);

            if(user == null)
            {
                return null;
            }
            
            if(!VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

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
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = true;
            }
            await _context.SaveChangesAsync();
        }


        public async Task UnblockUsersAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsersAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task MakeAdminsAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users)
            {
                user.Role = "Admin";
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAdminsAsync(List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users)
            {
                user.Role = UserRole.User;
            }
            await _context.SaveChangesAsync();
        }

        public async Task LogoutAsync(Guid id)
        {
           User? user = await GetUserByIdAsync(id);

            if (user != null)
            {
                user.IsActive = false;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<bool> IsBlocked(Guid id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
            {
                return true;
            }
            else
            {
                return user.IsBlocked;
            }
  
        }



    }
}
