using callapptask.Entitys;
using callapptask.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testtask.Models;

namespace testtask.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IUserInfo _rep;
        public readonly IConfiguration _config;
        public AuthController(IUserInfo repo, IConfiguration config)
        {
            _rep = repo ?? throw new ArgumentNullException(nameof(repo));
            _config = config ?? throw new ArgumentNullException(nameof(config));        }

        private int? Authenticate(LoginDto userData) 
        {
            var PossibleUser = _rep.GetUserByEmailAsync(userData.Email).Result;
            if (PossibleUser != null && PossibleUser.Password == userData.Password)
            {
                return PossibleUser.Id;
            }
            return null;
        }
        private string GenerateToken(LoginDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<User>> Register(UserDto input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            User user = new User();
            user.Email = input.email;
            user.Password = input.password;
            user.Username = input.username;

            await _rep.CreateUserAsync(user);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user); 
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(LoginDto input)
        {
            if (input.Email == null || input.Password == null)
            {
                return BadRequest("Invalid request payload.");
            }

            int? user = Authenticate(input);

            if (user != null) 
            {
                string token = GenerateToken(input);
                await _rep.SetActiveAsync(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }
    }
}
