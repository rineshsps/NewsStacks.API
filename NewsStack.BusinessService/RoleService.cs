using NewsStacks.Database.Models;
using NewsStacks.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsStacks.BusinessService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Role>> GetAllRole()
        {
            return await _repository.GetAllRole();
        }
    }
}
