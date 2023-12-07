using callapptask.DbContxt;
using callapptask.Entitys;
using Microsoft.EntityFrameworkCore;
using testtask.Models;

namespace callapptask.Repo
{
    public class UserInfo : IUserInfo
    {
        public readonly Context _context;
        public UserInfo(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserApiDto>> GetUsersAsync()
        {
            var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();

            var userDtos = users.Select(u => new UserApiDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive,
            });

            return userDtos;
        }

        public async Task SetActiveAsync(int? id)
        {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsActive = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
