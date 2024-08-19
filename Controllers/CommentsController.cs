using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using PCM.AutomapperMappingProfile;
using PCM.Data;
using PCM.ElasticSearchModels;
using PCM.Models;
using PCM.Services;

namespace PCM.Controllers
{
    public class CommentsController : Controller
    {
        private static List<Comment> _comments = new List<Comment>();

        private readonly AppDbContext _context;

        private readonly ElasticsearchService _elasticsearchService;

        public CommentsController( AppDbContext appContext, ElasticsearchService elasticsearchService)
        {
            _context = appContext;
            _elasticsearchService = elasticsearchService;
        }

        [HttpPost]
        public async Task<IActionResult>  AddComment(string itemid, string userName, string message)
        {
            var comment = new Comment
            {
                CommentID = Guid.NewGuid(),
                ItemId = Guid.Parse(itemid),    
                UserName = userName,
                Message = message,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            EsComment target = mapper.Map<EsComment>(comment);
            await _elasticsearchService.CreateIndexIfNotExists("comment-index");
            var result = await _elasticsearchService.AddOrUpdate(target, target.ItemId);

            return Ok(new { success = true, message = "Comment added successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Guid itemid)
        {
            if (itemid == Guid.Empty)
            {
                return BadRequest("Invalid item ID.");
            }

            var comments = await _context.Comments
                                         .Where(i => i.ItemId == itemid)
                                         .OrderBy(c => c.CreatedAt)
                                         .AsNoTracking()
                                         .ToListAsync();

            return Json(comments);
        }
    }
}
