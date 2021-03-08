using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsStacks.Database.Models;
using System.Collections.Generic;
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
    }
}
