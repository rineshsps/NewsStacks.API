using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsStacks.BusinessService;
using NewsStacks.Database.Models;
using System;
using System.Threading.Tasks;

namespace NewsStacks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly newsContext _context;
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _service;

        public RoleController(newsContext context, ILogger<RoleController> logger, IRoleService service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }


        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var model = await _service.GetAllRole();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get Roles");

                return BadRequest();
            }
        }

    }
}
