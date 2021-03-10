using Microsoft.EntityFrameworkCore;
using NewsStacks.Database.Models;
using NewsStacks.DTOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsStacks.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly newsContext _context;

        public ArticleRepository(newsContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Article article, string role)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return article.Id;
        }

        public async Task CreateArticleUser(string role, string userId, int articleId)
        {
            var userRole = _context.UserRoles.Where(x => x.UserId == Convert.ToInt32(userId) && x.RoleId == Convert.ToInt32(role)).FirstOrDefault();

            if (userRole == null)
            {
                var articleUser = new ArticleUser
                {
                    ArticleId = articleId,
                    UserRoleId = userRole.Id,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.ArticleUsers.Add(articleUser);
                await _context.SaveChangesAsync();

                // _logger.LogInformation($"Article Role {userRole.Id} saved.");
            };
        }

        public async Task<bool> Delete(int id)
        {
            var article = await _context.Articles.SingleAsync(x => x.Id == id && x.Active == true);
            article.Active = false;
            article.UpdateDate = DateTime.Now;
            _context.Entry(article).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Article>> GetAll(string role, bool published)
        {

            if (Convert.ToInt32(role) == (int)RoleType.Reader)
            {
                return await _context.Articles.Where(x => x.Active == true && x.PublishDone == true).OrderByDescending(x => x.PublishedDate).ToListAsync();
            }

            return await _context.Articles.Where(x => x.Active == true && x.PublishDone == published).ToListAsync();
        }

        public async Task<Article> GetById(int id, string role)
        {

            if (Convert.ToInt32(role) == (int)RoleType.Reader)
            {
                return await _context.Articles.Where(x => x.Active == true && x.Id == id & x.PublishDone == true).FirstOrDefaultAsync();
            }

            return await _context.Articles.Where(x => x.Active == true && x.Id == id).SingleAsync();
        }

        public async Task<Article> Update(int id, Article article, string role, string userId)
        {
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return article;
        }

    }
}
