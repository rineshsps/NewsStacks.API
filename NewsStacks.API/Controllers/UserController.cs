using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsStacks.Database.Models;
using NewsStacks.DTOs;
using NewsStacks.DTOs.Enum;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsStacks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly newsContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(newsContext context, ILogger<UserController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> PostUser(RegistrationDTO registration)
        {
            try
            {
                var user = _mapper.Map<User>(registration);
                user.Active = true;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var model = _mapper.Map<RegistrationDTO>(user);

                return CreatedAtAction("GetUser", new { id = user.Id }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to User registration");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(string userName = "writer", string password = "password")
        {
            try
            {
                var user = _context.Users.Include(x => x.UserRoles).Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();

                if (user != null)
                {
                    var tokenString = GenerateJSONWebToken(user);
                    return Ok(tokenString);
                }
                else
                {
                    _logger.LogWarning($"User {userName} Not Found");

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Authenticate");
                throw;
            }

        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private string GenerateJSONWebToken(User user)
        {
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("passkeywordsdfgdsfgfdsg"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles.FirstOrDefault();
            var roleid = "";

            roleid = role == null ? Convert.ToInt32(RoleType.Reader).ToString() : role.RoleId.ToString();

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, roleid),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("sessionId", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            var token = new JwtSecurityToken(
                      issuer: "Issuer",
                      claims: claims,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
