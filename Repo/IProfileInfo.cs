using callapptask.Entitys;
using testtask.Models;

namespace testtask.Repo
{
    public interface IProfileInfo
    {
        Task<IEnumerable<UserProfile>> GetProfilesAsync();
        Task<UserProfile?> GetProfileByIdAsync(int userId);
        Task<UserProfile?> CreateProfileAsync(int UserId, ProfileDto profiledto);
        Task DeleteProfileAsync(int userId);
        Task<UserProfile?> UpdateProfileAsync(int userId, ProfileDto profile);
    }
}
