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

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(Guid collectionId)
        {
            var AllTagNames = _context.Tags.Select(tag => tag.Name).Distinct().ToList();
            ViewBag.AllTagNames = AllTagNames;

            ViewBag.CollectionId = collectionId;
            var collection = _context.Collections.FirstOrDefault(c => c.CollectionId == collectionId);

            return View(new Item { CollectionId = collectionId, Collection = collection });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            // Generate new Guid for ItemId and set creation timestamp
            item.ItemId = Guid.NewGuid();
            item.CreatedAt = DateTime.Now;

            // Retrieve collection and associated user details
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == item.CollectionId);
            if (collection == null)
            {
                return NotFound("Collection not found.");
            }
                
            item.CollectionName = collection.Name;

            collection.TotalItems++;
            _context.Collections.Update(collection);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == collection.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            item.Author = user.Name;

            // Add the item to the context
            _context.Items.Add(item);

            // Process tags, skipping the first one
            foreach (var str in item.tags.Skip(1))
            {
                var tags = JsonConvert.DeserializeObject<List<string>>(str);
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

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the details page of the collection
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var AllTagNames = await _context.Tags.Select(tag => tag.Name).Distinct().ToListAsync();
            ViewBag.AllTagNames = AllTagNames;

            var CurrentItemsTags = await _context.Tags
                          .Where(t => t.ItemId == id)
                          .Select(t => t.Name)
                          .Distinct()
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
