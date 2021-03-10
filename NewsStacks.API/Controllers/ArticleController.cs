using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsStacks.API.Attribute;
using NewsStacks.BusinessService;
using NewsStacks.Database.Models;
using NewsStacks.DTOs;
using NewsStacks.DTOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IArticleService _service;

        public ArticleController(newsContext context, ILogger<ArticleController> logger, IMapper mapper, IArticleService service)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDisplayDTO>>> GetArticles(bool published = false)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role).Value;

                var articles = await _service.GetAll(role, published);
                var model = _mapper.Map<List<ArticleDisplayDTO>>(articles);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get Articles");

                return BadRequest();
            }
        }

        // GET: api/Article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role).Value;

                var article = await _service.GetById(id, role);

                if (article == null)
                {
                    return NotFound();
                }

                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get Articles");

                return BadRequest();
            }

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

                await _service.Update(id, article, role, userId);

                return Ok(article);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Articlde id {id} not found ");
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Article
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    article = await _service.Create(article, role, userId);
                    _logger.LogInformation($"Article saved.");
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

        // DELETE: api/Article/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete article");

                return BadRequest();
            }
        }
    }
}
