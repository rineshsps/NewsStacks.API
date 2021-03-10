using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsStacks.Database.Models;
using NewsStacks.DTOs.Enum;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsStacks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly newsContext _context;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(newsContext context, ILogger<UserRoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/UserRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoles()
        {
            return await _context.UserRoles.ToListAsync();
        }

        // GET: api/UserRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }


        // POST: api/UserRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UserRoleAssign")]
        public async Task<ActionResult<UserRole>> PostUserRole(int UserId, int RoleId)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role).Value;
                var user = new UserRole();
                if (Convert.ToInt32(role) == (int)RoleType.Admin)
                {
                    user = new UserRole
                    {
                        Active = true,
                        RoleId = RoleId,
                        UserId = UserId
                    };

                    _context.UserRoles.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Unauthorized();
                }

                return CreatedAtAction("GetUserRole", new
                {
                    id = user.Id
                }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to Save user Roles");

                return BadRequest();
            }

        }
    }
}
