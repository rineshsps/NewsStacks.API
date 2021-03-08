using Microsoft.EntityFrameworkCore;
using NewsStacks.Database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsStacks.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly newsContext _context;

        public RoleRepository(newsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRole()
        {
            return await _context.Roles.Where(x => x.Active == true).ToListAsync();
        }
    }
}
