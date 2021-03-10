using NewsStacks.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsStacks.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAll(string role, bool published);
        Task<Article> GetById(int id, string role);
        Task<int> Create(Article article, string role);
        Task<Article> Update(int id, Article article, string role, string userId);
        Task<bool> Delete(int id);
        Task CreateArticleUser(string role, string userId, int articleId);
    }
}
