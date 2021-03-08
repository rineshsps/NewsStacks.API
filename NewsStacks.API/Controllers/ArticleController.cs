using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NewsStacks.API.Attribute;
using NewsStacks.Database.Models;
using NewsStacks.DTOs;
using NewsStacks.DTOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsStacks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly newsContext _context;
        private readonly ILogger<ArticleController> _logger;
        private readonly IMapper _mapper;

        public ArticleController(newsContext context, ILogger<ArticleController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDisplayDTO>>> GetArticles(bool published = false)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (Convert.ToInt32(role) == (int)RoleType.Reader)
            {
                var articlesReader = await _context.Articles.Where(x => x.Active == true && x.PublishDone == true).OrderByDescending(x => x.PublishedDate).ToListAsync();
                var modelReader = _mapper.Map<List<ArticleDisplayDTO>>(articlesReader);

                return modelReader;
            }

            var articles = await _context.Articles.Where(x => x.Active == true && x.PublishDone == published).ToListAsync();
            var model = _mapper.Map<List<ArticleDisplayDTO>>(articles);
            return model;
        }

        // GET: api/Article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;

            var article = new Article { };

            if (Convert.ToInt32(role) == (int)RoleType.Reader)
            {
                article = await _context.Articles.Where(x => x.Active == true && x.Id == id & x.PublishDone == true).FirstOrDefaultAsync();
            }
            else
            {
                article = await _context.Articles.Where(x => x.Active == true && x.Id == id).FirstOrDefaultAsync();
            }

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Article/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            try
            {
                if (id != article.Id)
                {
                    _logger.LogError($"Id {id} and article id {article.Id} mismatch ");

                    return BadRequest();
                }

                var role = User.FindFirst(ClaimTypes.Role).Value;
                var userId = User.FindFirst("Id").Value;

                var model = await _context.Articles.Where(x => x.Active == true && x.Id == id & x.PublishDone == false).FirstOrDefaultAsync();

                if (model == null)
                {
                    _logger.LogError($"Article is not available/ is published ");

                    return BadRequest();
                }

                if (Convert.ToInt32(role) == (int)RoleType.Writer)
                {
                    if (model.WriteDone)
                    {
                        _logger.LogInformation($"Article Write completed already.");
                        return Ok();
                    }

                    model.Title = article.Title;
                    model.Description = article.Description;
                    model.Topics = article.Topics;
                    model.Tags = article.Tags;
                    model.UpdateDate = DateTime.UtcNow;
                    model.WriteDone = false;

                    if (!article.IsDraft)
                    {
                        model.WriteDone = true;
                        //Article message Queue 
                        var message = new ArticleMessage { Id = model.Id, Title = model.Title, MessageType = MessageType.WriterDone };
                        ArticleNotification(message);
                        _logger.LogInformation($"Article Notification message Writer added.");
                    }

                    await SaveArticleUser(role, userId, article.Id);

                }
                else if (Convert.ToInt32(role) == (int)RoleType.Reviewer)
                {
                    if (model.ReviewerDone)
                    {
                        _logger.LogInformation($"Article Reviewer completed already.");
                        return Ok();
                    }


                    model.ReviewerDone = true;
                    model.UpdateDate = DateTime.UtcNow;

                    //Article message Queue 
                    var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.ReviewerDone };
                    ArticleNotification(message);
                    _logger.LogInformation($"Article Notification message Reviewer added.");

                    await SaveArticleUser(role, userId, article.Id);

                }
                else if (Convert.ToInt32(role) == (int)RoleType.Editor)
                {
                    if (model.EditorDone)
                    {
                        _logger.LogInformation($"Article Reviewer completed already.");
                        return Ok();
                    }

                    model.EditorDone = true;
                    model.UpdateDate = DateTime.UtcNow;
                    model.EditorComments = article.EditorComments;
                    //Article message Queue 
                    var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.EditorDone };
                    ArticleNotification(message);
                    _logger.LogInformation($"Article Notification message Editor added.");

                    await SaveArticleUser(role, userId, article.Id);

                }
                else if (Convert.ToInt32(role) == (int)RoleType.Publisher)
                {
                    if (model.PublishDone)
                    {
                        _logger.LogInformation($"Article Reviewer completed already.");
                        return Ok();
                    }

                    model.PublishDone = true;
                    model.UpdateDate = DateTime.UtcNow;
                    model.PublishedDate = DateTime.UtcNow;
                    model.Tags = article.Tags;

                    //Article message Queue 
                    var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.PublisherDone };
                    ArticleNotification(message);
                    _logger.LogInformation($"Article Notification message Publisher added.");

                    await SaveArticleUser(role, userId, article.Id);

                }

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ArticleExists(id))
                {
                    _logger.LogError($"Articlde id {id} not found ");

                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Articlde id {id} not found ");
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Article
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save article, only writer can save 
        /// </summary>
        /// <param name="article"></param>
        /// <returns>article</returns>
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(ArticleDTO articleDTO)
        {
            try
            {
                //validation 
                if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState);

                var role = User.FindFirst(ClaimTypes.Role).Value;
                var userId = User.FindFirst("Id").Value;

                var article = _mapper.Map<Article>(articleDTO);

                if (Convert.ToInt32(role) == (int)RoleType.Writer)
                {
                    article.ReviewerDone = false;
                    article.PublishDone = false;
                    article.Active = true;
                    article.WriteDone = true;
                    if (article.IsDraft)
                    {
                        article.WriteDone = false;
                    }
                    article.CreatedDate = DateTime.UtcNow;
                    article.UpdateDate = DateTime.UtcNow;
                    article.PublishedDate = null;

                    _context.Articles.Add(article);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Article saved.");

                    //Save user information in Article role table 
                    await SaveArticleUser(role, userId, article.Id);

                    //Article message Queue 
                    if (!article.IsDraft)
                    {
                        var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.WriterDone };
                        ArticleNotification(message);
                        _logger.LogInformation($"Article Notification message added.");
                    }

                }
                else
                {
                    _logger.LogInformation($"Article created Unauthorized.");

                    return Unauthorized();
                }

                var articleDisplay = _mapper.Map<ArticleDisplayDTO>(article);
                return CreatedAtAction("GetArticle", new { id = article.Id }, articleDisplay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to save article");

                return BadRequest();
            }
        }

        private async Task SaveArticleUser(string role, string userId, int articleId)
        {
            var userRole = _context.UserRoles.Where(x => x.UserId == Convert.ToInt32(userId) && x.RoleId == Convert.ToInt32(role)).FirstOrDefault();

            if (userRole != null)
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

                _logger.LogInformation($"Article Role {userRole.Id} saved.");

            };
        }

        private static void ArticleNotification(ArticleMessage message)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("qa1");
            queue.CreateIfNotExistsAsync();
            string messsage = JsonSerializer.Serialize(message);
            queue.AddMessageAsync(new CloudQueueMessage(messsage));
        }

        // DELETE: api/Article/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            article.Active = false;
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
