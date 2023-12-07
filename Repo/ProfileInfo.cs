using callapptask.DbContxt;
using callapptask.Entitys;
using Microsoft.EntityFrameworkCore;
using testtask.Models;

namespace testtask.Repo
{
    public class ProfileInfo : IProfileInfo
    {
        public readonly Context _context;
        public ProfileInfo(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserProfile?> UpdateProfileAsync(int userId, ProfileDto profile)
        {
            var user = await _context.Users.Include(u => u.UserProfile)
                                    .SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null && user.UserProfile != null)
            {
                var userProfile = user.UserProfile;

                userProfile.FirstName = profile.FirstName;
                userProfile.LastName = profile.LastName;
                userProfile.PersonalNumber = profile.PersonalNumber;

                await _context.SaveChangesAsync();

                return userProfile;
            }
            return null;
        }

        public async Task<IEnumerable<UserProfile>> GetProfilesAsync()
        {
            return await _context.Profiles.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task DeleteProfileAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.UserProfile)
                    .Where(u => u.Id == userId)
                    .FirstOrDefaultAsync();
            if (user != null && user.UserProfile != null)
            {
                _context.Profiles.Remove(user.UserProfile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserProfile?> GetProfileByIdAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.UserProfile)
                                .Where(u => u.Id == userId)
                                .FirstOrDefaultAsync();
            if (user != null && user.UserProfile != null) { return user.UserProfile; }
            return null;
        }

        public async Task<UserProfile?> CreateProfileAsync(int UserId, ProfileDto input)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .SingleOrDefaultAsync(u => u.Id == UserId);

            if (user != null)
            {
                if (user.UserProfile == null)
                {
                    var profile = new UserProfile
                    {
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        PersonalNumber = input.PersonalNumber,
                        UserId = UserId,
                    };

                    _context.Profiles.Add(profile);
                    await _context.SaveChangesAsync();
                    return profile;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
