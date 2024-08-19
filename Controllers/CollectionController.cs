using AutoMapper;
using Azure;
using Elastic.Clients.Elasticsearch.Nodes;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using PCM.AutomapperMappingProfile;
using PCM.Data;
using PCM.ElasticSearchModels;
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
        private readonly ElasticsearchService _elasticsearchService;
        private readonly UserService _userService;


        public CollectionController(AppDbContext context, CloudinaryUploader cloudinaryUploader, ElasticsearchService elasticsearchService, UserService userService )
        {
            _context = context;
            _cloudinaryUploader = cloudinaryUploader;
            _elasticsearchService = elasticsearchService;
            _userService = userService;
        }

        public async Task<IActionResult>  Index()
        {
            if (await IsAdmin()) 
            {
                return RedirectToAction("ManageUsers", "Admin");
            }

            var userid = HttpContext.Session.GetString("Id");

            if (userid == null)
            {
                RedirectToAction("Login", "Account");
            }

            return RedirectToAction("IndexByUserID", new { userid = userid });

        }

        public async Task<IActionResult> IndexByUserID(Guid userid)
        {
 
            if (await IsAdmin() || IsValidUser(userid) )
            {
                ViewBag.UserId = userid;

                var collections = await _context.Collections
                .Where(c => c.UserId == userid)
                .ToListAsync();

                return View(collections);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }


        [HttpGet]
        public async Task<IActionResult> CreateByUserIDAsync(Guid userid)
        {

            if (await IsAdmin() || IsValidUser(userid))
            {
                ViewBag.UserId = userid;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        public async Task<bool> IsAdmin()
        {
            var sessionUserIdString = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(sessionUserIdString))
            {
                return false;
            }

            var sessionUserIdGuid = Guid.Parse(sessionUserIdString);

            return await _userService.IsAdminAsync(sessionUserIdGuid);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Collection collection, IFormFile Image)
        {

            if( await IsAdmin() || IsValidUser(collection.UserId)  )
            {
                string? ImageUrl = await Upload(Image);

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


                var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                var mapper = config.CreateMapper();
                EsCollection target = mapper.Map<EsCollection>(collection);
                await _elasticsearchService.CreateIndexIfNotExists("collection-index");
                var result = await _elasticsearchService.AddOrUpdate(target, target.CollectionId);

                return RedirectToAction("IndexByUserID", new { userid = collection.UserId });
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        public async Task<IActionResult> Details(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);

            if (collection == null)
            {
                return NotFound();
            }

            if (await IsAdmin() || IsValidUser(collection.UserId) )
            {
                return View(collection);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

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
            Collection? collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == id);

            if(collection == null)
            {
                return NotFound();
            }

            if (await IsAdmin() || IsValidUser(collection.UserId)  )
            {
                if(collection.ImageUrl != null)
                {
                    await _cloudinaryUploader.RemoveImageAsync(collection.ImageUrl);
                }

                _context.Collections.Remove(collection);

                var items = await _context.Items.Where(i => i.CollectionId == id).ToListAsync();

                foreach (var item in items)
                {
                    _elasticsearchService.SetIndex("item-index");
                    await _elasticsearchService.Remove<dynamic>(item.ItemId.ToString());
                }

                foreach (var item in items)
                {
                    _elasticsearchService.SetIndex("comment-index");
                    await _elasticsearchService.Remove<dynamic>(item.ItemId.ToString());
                }

                _elasticsearchService.SetIndex("collection-index");
                await _elasticsearchService.Remove<dynamic>(id.ToString());

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
           
            if(collection == null) return NotFound();

            if (await IsAdmin() || IsValidUser(collection.UserId) )
            {
                return View(collection);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
  
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Collection collection, IFormFile Image)
        {
            if (collection == null) return NotFound();

            if (await IsAdmin() || IsValidUser(collection.UserId) )
            {
                try
                {
                    if (Image != null)
                    {
                        string? imageUrl = await Upload(Image);
                        collection.ImageUrl = imageUrl;
                    }

                    _context.Entry(collection).State = EntityState.Modified;
                    await _context.SaveChangesAsync();


                    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                    var mapper = config.CreateMapper();
                    EsCollection target = mapper.Map<EsCollection>(collection);
                    _elasticsearchService.Index("collection-index");
                    var result = await _elasticsearchService.AddOrUpdate(target, target.CollectionId);
                }
                catch (Exception ex)
                {
                    Log.Information(ex.Message.ToString());
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        public async Task<IActionResult> AllCollections()
        {
            var collection = await _context.Collections.ToListAsync();
            return View(collection);
        }

        //Collection Details View for non authenticated users
        public async Task<IActionResult> DetailsPublic(Guid id)
        {
            var collection = await _context.Collections.Include(c => c.Items).FirstOrDefaultAsync(c => c.CollectionId == id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

      
        public bool IsValidUser(Guid userid)
        {
            var sessionUserIdString = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(sessionUserIdString))
            {
               return false;
            }
            var sessionUserIdGuid = Guid.Parse(sessionUserIdString);

            if (sessionUserIdGuid == userid)
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