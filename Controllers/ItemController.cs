using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;

namespace PCM.Controllers
{
    public class ItemController : Controller
    {


        private readonly AppDbContext _context;
        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int collectionId)
        {
            var collection = _context.Collections.FirstOrDefault(c => c.Id == collectionId);
            if (collection == null)
            {
                return NotFound();
            }

            var item = new Item { CollectionId = collectionId, Collection = collection };
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
            }

            item.Collection = _context.Collections.FirstOrDefault(c => c.Id == item.CollectionId);
            return View(item);
        }

        [HttpPost]
        public JsonResult Autocomplete(string prefix)
        {
            var tags = (from item in _context.Items
                        where item.Tags.Contains(prefix)
                        select new
                        {
                            value = item.Tags
                        }).ToList();

            return Json(tags);
        }


    }
}
