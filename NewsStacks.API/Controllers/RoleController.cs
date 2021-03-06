using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsStacks.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsStacks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly newsContext _context;
        private readonly ILogger<RoleController> _logger;

        public RoleController(newsContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                return await _context.Roles.Where(x => x.Active == true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get Roles");

                return BadRequest();
            }
        }

    }
}
