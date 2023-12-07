using callapptask.Entitys;
using testtask.Models;

namespace callapptask.Repo
{
    public interface IUserInfo
    {
        Task<IEnumerable<UserApiDto>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task SetActiveAsync(int? id);
    }
}
