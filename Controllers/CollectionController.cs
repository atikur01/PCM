using Azure;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;
using PCM.Services;
using PCM.ViewModels;
using Serilog;
using System;

namespace PCM.Controllers
{
    public class CollectionController : Controller
    {

        private readonly AppDbContext _context;
        private readonly CloudinaryUploader _cloudinaryUploader;
     

        public CollectionController(AppDbContext context, CloudinaryUploader cloudinaryUploader)
        {
            _context = context;
            _cloudinaryUploader = cloudinaryUploader;
        }

        public IActionResult Index()
        {
            var collections = _context.Collections.Include(c => c.Items).ToList();
            
            return View(collections);
        }

        public async Task<IActionResult> IndexByUserID(Guid userid)
        {
            ViewBag.UserId = userid;

            var collections = await _context.Collections
            .Where(c => c.UserId == userid)
            .ToListAsync();

            return View(collections);

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateByUserID(Guid userid)
        {
            ViewBag.UserId = userid;    
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Collection collection, IFormFile Image)
        {
            string ImageUrl = await Upload(Image);

            if(collection.Description == null)
            {
                collection.Description = "";
            }   

            var htmlContent = Markdown.ToHtml(collection.Description);
            collection.Description = htmlContent;
            collection.CollectionId = Guid.NewGuid();
            collection.ImageUrl = ImageUrl;
            collection.CreatedAt = DateTime.Now;
            collection.TotalItems = 0;  
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexByUserID", new { userid = collection.UserId });


        }

        public IActionResult Details(Guid id)
        {
            var collection = _context.Collections.Include(c => c.Items).FirstOrDefault(c => c.CollectionId == id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        


        public async Task<string?> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "File is empty.";
            }

            string publicUrl = await _cloudinaryUploader.UploadFileAsync(file);

            if (publicUrl == null)
            {
                return "Error uploading file to Cloudinary.";
            }

            return publicUrl;
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var collection = _context.Collections.FirstOrDefault(c => c.CollectionId == id);
            await _cloudinaryUploader.RemoveImageAsync(collection.ImageUrl);
            _context.Collections.Remove(collection);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);

            return View(collection);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Collection collection,IFormFile Image)
        {
     
            try
            {
                // Upload image if exists
                if (Image != null)
                {
                    string imageUrl = await Upload(Image);
                    collection.ImageUrl = imageUrl;
                }

                _context.Update(collection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! CollectionExists(collection.CollectionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        private bool CollectionExists(Guid id)
        {
            return _context.Collections.Any(e => e.CollectionId == id);
        }

    }
}
