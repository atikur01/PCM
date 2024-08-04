using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Index(int collectionId)
        {
            var items = _context.Items
                .Where(i => i.CollectionId == collectionId)
                .Include(i => i.Collection)
                .ToList();
            ViewBag.CollectionId = collectionId;
            return View(items);
        }

        public IActionResult Create(int collectionId)
        {
            ViewBag.CollectionId = collectionId;
            var collection = _context.Collections.FirstOrDefault(c => c.Id == collectionId);
            if (collection == null)
            {
                return NotFound();
            }
            return View(new Item { CollectionId = collectionId, Collection = collection });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { collectionId = item.CollectionId });
            
            var collection = _context.Collections.FirstOrDefault(c => c.Id == item.CollectionId);
            item.Collection = collection;
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            var collection = _context.Collections.FirstOrDefault(c => c.Id == item.CollectionId);
            ViewBag.Collection = collection;
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { collectionId = item.CollectionId });
            }
            return View(item);
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { collectionId = item.CollectionId });
        }

        public IActionResult AutocompleteTags(string term)
        {
            var tags = _context.Items
                .Where(i => i.Tag1.StartsWith(term) || i.Tag2.StartsWith(term) || i.Tag3.StartsWith(term) || i.Tag4.StartsWith(term) || i.Tag5.StartsWith(term))
                .SelectMany(i => new[] { i.Tag1, i.Tag2, i.Tag3, i.Tag4, i.Tag5 })
                .Where(t => t != null && t.StartsWith(term))
                .Distinct()
                .Take(10)
                .ToList();
            return Json(tags);
        }


    }
}
