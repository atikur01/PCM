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
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                return RedirectToAction("ManageUsers", "Admin");
            }

            var userid = HttpContext.Session.GetString("Id");

            if (userid == null) return RedirectToAction("Login", "Account");

            return RedirectToAction("IndexByUserID", new { userid = userid });

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

            if (collection.Description == null)
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

        public async Task<IActionResult> Details(Guid id)
        {


            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
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
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == id);
            await _cloudinaryUploader.RemoveImageAsync(collection.ImageUrl);
            _context.Collections.Remove(collection);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = await _context.Collections
                .Where(c => c.CollectionId == id)
                .Select(c => c.UserId)
                .FirstOrDefaultAsync();

            ViewBag.UserId = userId;


            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
            ViewBag.CreatedAt = collection.CreatedAt;

            return View(collection);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Collection collection, IFormFile Image)
        {

            try
            {
                // Upload image if exists
                if (Image != null)
                {
                    string imageUrl = await Upload(Image);
                    collection.ImageUrl = imageUrl;
                }
                // Mark the entity as modified
                _context.Entry(collection).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> AllCollections()
        {
            var collection = await _context.Collections.ToListAsync();
            return View(collection);
        }

        public async Task<IActionResult> DetailsView(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }


        public async Task<IActionResult> CollectionDetails(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);


        }

    }
}