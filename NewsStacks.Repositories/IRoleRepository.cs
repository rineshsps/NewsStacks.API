using NewsStacks.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsStacks.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRole();
    }
}
