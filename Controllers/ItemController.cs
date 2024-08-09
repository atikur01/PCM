using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PCM.Data;
using PCM.Models;
using Serilog;

namespace PCM.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;
        // Example in-memory storage of tags for demonstration purposes
        private static List<string> _tags = new List<string> { "example", "tag", "test" };

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index(Guid collectionId)
        //{

        //    var items = _context.Items
        //        .Where(i => i.CollectionId == collectionId)
        //        .Include(i => i.Collection)
        //        .ToList();
        //    ViewBag.CollectionId = collectionId;
        //    return View(items);
        //}

        public IActionResult Create(Guid collectionId)
        {
            var AllTagNames = _context.Tags.Select(tag => tag.Name).ToList();
            ViewBag.AllTagNames = AllTagNames;

            ViewBag.CollectionId = collectionId;
            var collection = _context.Collections.FirstOrDefault(c => c.CollectionId == collectionId);

            return View(new Item { CollectionId = collectionId, Collection = collection });
        }

        [HttpPost]
        public async Task<IActionResult> Create( Item item )
        {
            item.ItemId = Guid.NewGuid();
            _context.Items.Add(item);

            int count = 0;  
            foreach (var str in item.tags)
            {
                if (count == 0) 
                { 
                    count++;
                    continue;

                }
                else
                {
                    List<string> tags = JsonConvert.DeserializeObject<List<string>>(str);

                    // Output the list
                    foreach (var tag in tags)
                    {
                        _context.Tags.Add(new Tag
                        {
                            TagId = Guid.NewGuid(),
                            ItemId = item.ItemId,
                            Name = tag

                        });
                    }


                }

            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var AllTagNames = await _context.Tags.Select(tag => tag.Name).ToListAsync();
            ViewBag.AllTagNames = AllTagNames;

            var CurrentItemsTags = await _context.Tags
                          .Where(t => t.ItemId == id)
                          .Select(t => t.Name)
                          .ToListAsync();

            ViewBag.CurrentItemsTags = CurrentItemsTags;

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == item.CollectionId);
            ViewBag.Collection = collection;

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            int count = 0;
            foreach (var str in item.tags)
            {
                if (count == 0)
                {
                    count++;
                    continue;

                }
                else
                {
                    _context.Tags.RemoveRange(_context.Tags.Where(t => t.ItemId == item.ItemId));   

                    List<string> tags = JsonConvert.DeserializeObject<List<string>>(str);

                    // Output the list
                    foreach (var tag in tags)
                    {
                        _context.Tags.Add(new Tag
                        {
                           
                            ItemId = item.ItemId,
                            Name = tag

                        });
                    }
                }
            }

            ViewBag.CollectionId = item.CollectionId;   
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
            _context.Items.Remove(item);

            var tags = await _context.Tags.Where(t => t.ItemId == id).ToListAsync();
            _context.Tags.RemoveRange(tags);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }





    }
}
