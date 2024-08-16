using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;
using PCM.ViewModels;
using System.Diagnostics;

namespace PCM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context; 

        public HomeController(ILogger<HomeController> logger , AppDbContext appContext )
        {
            _logger = logger;
            _context = appContext;  
        }

        public async Task<IActionResult>  Index()
        {
            var items = await _context.Items
                  .OrderByDescending(item => item.CreatedAt)
                  .Take(6)
                  .ToListAsync();

            var topCollections = await _context.Collections
                 .Include(c => c.Items)
                 .OrderByDescending(c => c.Items.Count)
                 .Take(5)
                 .ToListAsync();

            var topTags =await _context.Tags
                .GroupBy(t => t.Name)
                .Select(g => new
                {
                    TagName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t => t.Count)
                .Take(7)
                .ToListAsync();

            var taglist = new Dictionary<string, string>();
            int i = 1;
            foreach (var tag in topTags)
            {
                taglist.Add( "Tag"+i , tag.TagName);
                i++;
            }


            var viewModel = new HomeViewModel
            {
                Items = items,
                Collections = topCollections ,
                Tags = taglist
            };  

            return View(viewModel);
        }

       


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
