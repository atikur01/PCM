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

        public IActionResult Index(Guid collectionId)
        {
            var items = _context.Items
                .Where(i => i.CollectionId == collectionId)
                .Include(i => i.Collection)
                .ToList();
            ViewBag.CollectionId = collectionId;
            return View(items);
        }

        public IActionResult Create(Guid collectionId)
        {
            ViewBag.CollectionId = collectionId;
            var collection = _context.Collections.FirstOrDefault(c => c.CollectionId == collectionId);

            return View(new Item { CollectionId = collectionId, Collection = collection });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });

        }

        public IActionResult Edit(Guid id)
        {
            var item = _context.Items.FirstOrDefault(i => i.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            var collection = _context.Collections.FirstOrDefault(c => c.CollectionId == item.CollectionId);
            ViewBag.Collection = collection;
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });

        }

        public async Task<IActionResult>  Delete(Guid id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();  
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }

        
      


    }
}
