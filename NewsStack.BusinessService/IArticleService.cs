using NewsStacks.Database.Models;
using NewsStacks.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsStacks.BusinessService
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAll(string role, bool published);
        Task<Article> GetById(int id, string role);
        Task<Article> Create(Article article, string role, string userId);
        Task<Article> Update(int id, Article article, string role, string userId);
        bool CreateArticleNotification(ArticleMessage message);
        Task<bool> Delete(int id);
    }
}
