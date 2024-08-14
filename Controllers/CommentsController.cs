using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;

namespace PCM.Controllers
{
    public class CommentsController : Controller
    {
        private static List<Comment> _comments = new List<Comment>();

        private readonly AppDbContext _context;

        public CommentsController( AppDbContext appContext)
        {
            _context = appContext;
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
            _context.SaveChangesAsync();

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
                                         .OrderBy(c => c.CreatedAt) // Assuming there's a CreatedDate property
                                         .AsNoTracking()
                                         .ToListAsync();

            return Json(comments);
        }
    }
}
