using atikapps;
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
        private  UserService _userService;
        private readonly bool Esflag = false;

        public CommentsController( AppDbContext appContext, ElasticsearchService elasticsearchService, UserService userService)
        {
            _context = appContext;
            _elasticsearchService = elasticsearchService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult>  AddComment(string itemid, string userName, string message, Guid visitorID)
        {
            if (string.IsNullOrEmpty(itemid) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Invalid input.");
            }

            if( await IsValidUserAsync(visitorID))
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

                if (Esflag)
                {
                    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                    var mapper = config.CreateMapper();
                    EsComment target = mapper.Map<EsComment>(comment);
                    await _elasticsearchService.CreateIndexIfNotExists("comment-index");
                    var result = await _elasticsearchService.AddOrUpdate(target, target.ItemId);

                }



                return Ok(new { success = true, message = "Comment added successfully" });

            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
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



        public async Task<bool> IsValidUserAsync(Guid userid)
        {
            var sessionUserIdString = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(sessionUserIdString))
            {
                return false;
            }
            var sessionUserIdGuid = Guid.Parse(sessionUserIdString);

            var isUserExist = await _userService.GetUserByIdAsync(userid);

            var IsUserBlocked = await _userService.IsBlocked(userid);

            if (sessionUserIdGuid == userid && isUserExist != null && IsUserBlocked==false)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
