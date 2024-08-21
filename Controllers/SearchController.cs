
using CloudinaryDotNet.Core;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
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
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }

            var itemIds = new HashSet<string>();
            var items = new List<Item>();

            var collectionIds = new HashSet<string>();
            var collections = new List<Collection>();

            var query = new SimpleQueryStringQuery
            {
                Query = keyword,
                DefaultOperator = Operator.Or
            };

            // Search items
            _elasticsearchService.Index("item-index");
            var resultsItems = await _elasticsearchService.Query<dynamic>(query);

            if (resultsItems != null)
            {
                foreach (var result in resultsItems)
                {
                    if (result.TryGetValue("item_id", out object itemIdValue))
                    {
                        var itemId = itemIdValue.ToString();
                        itemIds.Add(itemId);
                    }
                }
            }

            // Search comments
            _elasticsearchService.Index("comment-index");
            var resultComments = await _elasticsearchService.Query<dynamic>(query);

            if (resultComments != null)
            {
                foreach (var result in resultComments)
                {
                    if (result.TryGetValue("item_id", out object itemIdValue))
                    {
                        var itemId = itemIdValue.ToString();
                        itemIds.Add(itemId);
                    }
                }
            }

            // Fetch distinct items
            if (itemIds.Count > 0)
            {
                foreach (var itemId in itemIds)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == Guid.Parse(itemId));
                    if (item != null)
                    {
                        items.Add(item);
                    }
                }

                items = items.Distinct().ToList();
            }

            // Search collections
            _elasticsearchService.Index("collection-index");
            var resultCollection = await _elasticsearchService.Query<dynamic>(query);

            if (resultCollection != null)
            {
                foreach (var result in resultCollection)
                {
                    if (result.TryGetValue("collection_id", out object collectionIdValue))
                    {
                        var collectionId = collectionIdValue.ToString();
                        collectionIds.Add(collectionId);
                    }
                }
            }

            // Fetch distinct collections
            if (collectionIds.Count > 0)
            {
                foreach (var collectionId in collectionIds)
                {
                    var collection = await _context.Collections.FirstOrDefaultAsync(i => i.CollectionId == Guid.Parse(collectionId));
                    if (collection != null)
                    {
                        collections.Add(collection);
                    }
                }

                collections = collections.Distinct().ToList();
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
    