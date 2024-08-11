using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PCM.Data;
using PCM.Models;
using PCM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCM.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(Guid collectionId)
        {
            var allTagNames = await  _context.Tags.Select(tag => tag.Name).Distinct().ToListAsync();
            ViewBag.AllTagNames = allTagNames;

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == collectionId);
            if (collection == null)
            {
                return NotFound("Collection not found.");
            }

            var newItem = new Item
            {
                CollectionId = collectionId,
                Collection = collection
            };

            return View(newItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            if (item == null)
            {
                return BadRequest("Invalid item data.");
            }

            item.ItemId = Guid.NewGuid();
            item.CreatedAt = DateTime.Now;

            var collection = await _context.Collections.FindAsync(item.CollectionId);
            if (collection == null)
            {
                return NotFound("Collection not found.");
            }

            item.CollectionName = collection.Name;
            collection.TotalItems++;
            _context.Collections.Update(collection);

            var user = await _context.Users.FindAsync(collection.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            item.Author = user.Name;

            _context.Items.Add(item);

            // Process and add tags, skipping the first tag in the list
            ProcessTags(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var allTagNames = await _context.Tags.Select(tag => tag.Name).Distinct().ToListAsync();
            ViewBag.AllTagNames = allTagNames;

            var currentItemsTags = await _context.Tags
                .Where(t => t.ItemId == id)
                .Select(t => t.Name)
                .Distinct()
                .ToListAsync();
            ViewBag.CurrentItemsTags = currentItemsTags;

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound("Item not found.");
            }

            var collection = await _context.Collections.FindAsync(item.CollectionId);
            ViewBag.Collection = collection;

            var user = await _context.Users.FindAsync(collection.UserId);

            ViewBag.CollectionName = collection.Name;   
            ViewBag.CreatedAt = item.CreatedAt; 

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            if (item == null)
            {
                return BadRequest("Invalid item data.");
            }

            _context.Tags.RemoveRange(_context.Tags.Where(t => t.ItemId == item.ItemId));
            ProcessTags(item);

            _context.Items.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound("Item not found.");
            }

            _context.Items.Remove(item);

            var tags = await _context.Tags.Where(t => t.ItemId == id).ToListAsync();
            _context.Tags.RemoveRange(tags);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
        }

        private void ProcessTags(Item item)
        {
            foreach (var str in item.tags.Skip(1)) // Skip the first tag
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
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound("Item not found.");
            }

            var tags = await _context.Tags
                .Where(t => t.ItemId == id)
                .Select(t => t.Name)
                .Distinct()
                .ToListAsync();
            ViewBag.Tags = tags;

            var collection = await _context.Collections.FindAsync(item.CollectionId);

            var user = await _context.Users.FindAsync(collection.UserId);

            var ItemDetailsViewModel = new ItemDetailsViewModel
            {
                item = item,
                collection = collection,
                AuthorName = user.Name  
            };  

           

            return View(ItemDetailsViewModel);
        }   

    }
}
