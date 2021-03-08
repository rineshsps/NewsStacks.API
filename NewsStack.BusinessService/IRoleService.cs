using NewsStacks.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsStacks.BusinessService
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRole();
    }
}
