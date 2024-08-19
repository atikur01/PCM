
using CloudinaryDotNet.Core;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Nest;
using Newtonsoft.Json;
using PCM.Data;
using PCM.ElasticSearchModels;
using PCM.Models;
using PCM.Services;
using PCM.ViewModels;
using Serilog;
using System.Reflection;

namespace PCM.Controllers
{
    public class SearchController : Controller
    { 
        private readonly AppDbContext _context;
        private readonly ElasticsearchService _elasticsearchService;

        public SearchController(AppDbContext context, ElasticsearchService elasticsearchService)
        {
            _context = context;
            _elasticsearchService = elasticsearchService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keyword)
        {
            if(string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }   

            var itemids = new List<string>();
            var items = new List<Item>();

            var collectionids = new List<string>();
            var collections = new List<Collection>();

            var query = new SimpleQueryStringQuery
            {
                Query = keyword,
                DefaultOperator = Operator.And // Use AND to match all terms
            };

            _elasticsearchService.Index("item-index");
            var resultsItems = await _elasticsearchService.Query<dynamic>(query);
            
            foreach (var result in resultsItems)
            {
                // Assuming 'result' is of type Dictionary<string, object>
                if (result.TryGetValue("item_id", out object itemIdValue))
                {
                    var itemId = itemIdValue.ToString();
                    itemids.Add(itemId);
                }
              
            }


            _elasticsearchService.Index("comment-index");
            var resultComments = await _elasticsearchService.Query<dynamic>(query);

            foreach (var result in resultComments)
            {
                if (result.TryGetValue("item_id", out object itemIdValue))
                {
                    var itemId = itemIdValue.ToString();
                    itemids.Add(itemId);
                }

            }

            foreach (var itemid in itemids)
            {
                var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == Guid.Parse(itemid));
                 if (item != null)  
                items.Add(item);
            }


            _elasticsearchService.Index("collection-index");
            var resultCollection = await _elasticsearchService.Query<dynamic>(query);

            foreach (var result in resultCollection)
            {
                if (result.TryGetValue("collection_id", out object collectionIdValue))
                {
                    var collectionId = collectionIdValue.ToString();
                    collectionids.Add(collectionId);
                }

            }

            foreach (var collectionid in collectionids)
            {
                var item = await _context.Collections.FirstOrDefaultAsync(i => i.CollectionId == Guid.Parse(collectionid));
                collections.Add(item);
            }


            var searchResults = new SearchViewModel
            {
                Items = items,
                Collections = collections,  
                Keyword = keyword
            };


            return View(searchResults); 
        }
    }
}
    