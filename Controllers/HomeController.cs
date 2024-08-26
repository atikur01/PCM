using atikapps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;
using PCM.Services;
using PCM.ViewModels;
using System.Diagnostics;

namespace PCM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;

        private readonly ElasticsearchService _elasticsearchService;

        public HomeController(ILogger<HomeController> logger , AppDbContext appContext, ElasticsearchService elasticsearchService)
        {
            _logger = logger;
            _context = appContext;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<IActionResult>  Index()
        {
            var ItemsToTake = 6;
            var items = await _context.Items
                  .OrderByDescending(item => item.CreatedAt)
                  .Take(ItemsToTake)
                  .ToListAsync();


            var CollectionToTake = 5;
            var topCollections = await _context.Collections
                 .Include(c => c.Items)
                 .OrderByDescending(c => c.Items.Count)
                 .Take(CollectionToTake)
                 .ToListAsync();


            var TagToTake = 7;
            var topTags =await _context.Tags
                .GroupBy(t => t.Name)
                .Select(g => new
                {
                    TagName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t => t.Count)
                .Take(TagToTake)
                .ToListAsync();


            var taglist = new Dictionary<string, string>();
            

            int i = 1;
            foreach (var tag in topTags)
            {
                if(tag.TagName != null){
                    taglist.Add("Tag" + i, tag.TagName);
                    i++;
                }
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

        public IActionResult AccessDenied()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        


    }


  
}
