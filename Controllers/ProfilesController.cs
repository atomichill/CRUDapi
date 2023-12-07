using callapptask.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testtask.Models;
using testtask.Repo;

namespace testtask.Controllers
{
    [Authorize]
    [Route("api/Profiles")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        public readonly IProfileInfo _rep;
        public ProfilesController(IProfileInfo rep)
        {
            _rep = rep ?? throw new ArgumentNullException(nameof(rep));
        }


        [HttpGet]
        public async Task<ActionResult<List<UserProfile>>> GetProfiles()
        {
            var profiles = await _rep.GetProfilesAsync();
            if (profiles == null)
            {
                return NotFound("There are no Profiles");
            }

            return Ok(profiles);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<UserProfile>> CreateProfile(int UserId, ProfileDto input)
        {
            if (UserId == 0 || input.FirstName == null || input.LastName == null || input.PersonalNumber.ToString() == null)
            {
                return BadRequest();
            }
            var createdProfile = await _rep.CreateProfileAsync(UserId,input);
            if (createdProfile == null) 
            {
                return StatusCode(500);
            }
            return CreatedAtAction(nameof(CreateProfile), new { id = createdProfile.Id }, createdProfile);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdateProfile(int userId, ProfileDto profile) 
        {
            if (userId == 0 || profile.FirstName == null || profile.LastName == null || profile.PersonalNumber.ToString() == null)
            {
                return BadRequest();
            }
            var updatedprof = await _rep.UpdateProfileAsync(userId, profile);
            if(updatedprof == null) { return NotFound(); };

            return Ok(updatedprof);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeleteProfile(int userId)
        {
            await _rep.DeleteProfileAsync(userId);
            return NoContent();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult> GetProfileById(int userId)
        {
            var profile = await _rep.GetProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
    }
}
