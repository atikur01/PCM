﻿using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PCM.Data;
using PCM.Models;
using PCM.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using PCM.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.Net;
using Elastic.Clients.Elasticsearch.Nodes;
using Nest;
using AutoMapper;
using PCM.AutomapperMappingProfile;
using PCM.ElasticSearchModels;

namespace PCM.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ElasticsearchService _elasticsearchService;
        private readonly UserService _userService;

        public ItemController(AppDbContext context, ElasticsearchService elasticsearchService, UserService userService ) 
        {
            _context = context;
            _elasticsearchService = elasticsearchService;
            _userService = userService;

        }

        public async Task<IActionResult> Create(Guid collectionId)
        {
            var allTagNames = await _context.Tags.Select(tag => tag.Name).Distinct().ToListAsync();
            ViewBag.AllTagNames = allTagNames;

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == collectionId);
            if (collection == null)
            {
                return NotFound("Collection not found.");
            }

            if (await IsAdmin() || await IsValidUserAsync(collection.UserId)  )
            {
                var newItem = new Item
                {
                    CollectionId = collectionId,
                    Collection = collection
                };
                return View(newItem);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

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

            if (await IsAdmin() || await IsValidUserAsync(collection.UserId) )
            {
                item.CollectionName = collection.Name;
                collection.TotalItems++;
                _context.Collections.Update(collection);

                var user = await _userService.GetUserByIdAsync(collection.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                item.Author = user.Name;
                item.UserId = collection.UserId;

                _context.Items.Add(item);


                var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                var mapper = config.CreateMapper();
                EsItem target = mapper.Map<EsItem>(item);
                await _elasticsearchService.CreateIndexIfNotExists("item-index");
                await _elasticsearchService.AddOrUpdate(target, target.ItemId);

                // Process and add tags, skipping the first tag in the list
                ProcessTags(item);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
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

            if(collection == null)
            {
                return NotFound("Collection not found.");
            }   

            if (await IsAdmin() || await IsValidUserAsync(collection.UserId) )
            {
                ViewBag.Collection = collection;

                var user = await _context.Users.FindAsync(collection.UserId);

                if(user == null)
                {
                    return NotFound("User not found.");
                }

                ViewBag.CollectionName = collection.Name;
                ViewBag.CreatedAt = item.CreatedAt;
                ViewBag.UserId = user.UserId;

                return View(item);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

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

            if (await IsAdmin() || await IsValidUserAsync(item.UserId) )
            {
                _context.Items.Update(item);

                var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                var mapper = config.CreateMapper();
                EsItem target = mapper.Map<EsItem>(item);
                _elasticsearchService.Index("item-index");
                await _elasticsearchService.AddOrUpdate(target, target.ItemId);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            
            if (item == null)
            {
                return NotFound("Item not found.");
            }

            if (await IsAdmin() || await IsValidUserAsync(item.UserId) )
            {
                var collection = await _context.Collections.FindAsync(item.CollectionId);

                if (collection == null)
                {
                    return NotFound("Collection not found.");
                }
                else
                {
                    collection.TotalItems--;
                }

                _context.Items.Remove(item);

                _elasticsearchService.Index("item-index");
                await _elasticsearchService.Remove<dynamic>(id.ToString());

                var tags = await _context.Tags.Where(t => t.ItemId == id).ToListAsync();

                _elasticsearchService.Index("comment-index");
                await _elasticsearchService.Remove<dynamic>(id.ToString());

                _context.Tags.RemoveRange(tags);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Collection", new { id = item.CollectionId });
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }   
        }

        private void ProcessTags(Item item)
        {
            foreach (var str in item.tags.Skip(1)) // Skip the first tag
            {
                var tags = JsonConvert.DeserializeObject<List<string>>(str);
                foreach (var tag in tags)
                {
                    Tag tagobj = new Tag
                    {
                        TagId = Guid.NewGuid(),
                        ItemId = item.ItemId,
                        Name = tag
                    };
                    _context.Tags.Add(tagobj);
                }
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.Items
                .Include(i => i.Collection)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound("Item not found.");
            }

            var tags = await _context.Tags
                .Where(t => t.ItemId == id)
                .Select(t => t.Name)
                .Distinct()
                .ToListAsync();

            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.ItemId == id);

            Models.Like likeobj = null;

            if (like == null)
            {
                likeobj = new Models.Like
                {
                    ItemId = id
                };
            }

            Guid VisitorUserId = Guid.NewGuid();

            try
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    ViewBag.Liked = true;
                }
                else
                {
                    VisitorUserId = Guid.Parse(HttpContext.Session.GetString("Id"));
                    ViewBag.VisitorUserId = VisitorUserId;
                }

            }
            catch (Exception)
            {
                Log.Information("User not logged in");
            }


            var existingLike = await _context.Likes.AnyAsync(l => l.VisitorUserID == VisitorUserId && l.ItemId == id);

            if (existingLike)
            {
                ViewBag.Liked = true;
            }
            else
            {
                ViewBag.Liked = false;
            }


            if (like != null)
            {
                likeobj = like;
            }

            // Create the ViewModel
            var itemDetailsViewModel = new ItemDetailsViewModel
            {
                item = item,
                collection = item.Collection,
                visitorID = VisitorUserId,
                AuthorName = item.Collection.User.Name,
                Tags = tags,
                Like = likeobj,
                CommenterName = HttpContext.Session.GetString("Name"),
                ItemLikeCount = new ItemLikeCount
                {
                    ItemId = item.ItemId,
                    LikeCount = await _context.ItemLikeCounts
                    .Where(ilc => ilc.ItemId == id)
                    .Select(ilc => ilc.LikeCount)
                    .FirstOrDefaultAsync()
                }
            };

            return View(itemDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Like(Models.Like like)
        {
            if (like == null)
            {
                return BadRequest("Invalid like data.");
            }

            if (await IsValidUserAsync( Guid.Parse(like.VisitorUserID.ToString() )  ))
            {
                like.LikeID = Guid.NewGuid();
                var itemid = Guid.Parse(like.ItemId.ToString());

                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.VisitorUserID == like.VisitorUserID && l.ItemId == like.ItemId);

                if (existingLike == null)
                {
 
                    _context.Likes.Add(like);


                    if (await _context.ItemLikeCounts.FirstOrDefaultAsync(l => l.ItemId == like.ItemId) == null)
                    {
                        _context.ItemLikeCounts.Add(new ItemLikeCount
                        {
                            ItemLikeCountId = Guid.NewGuid(),
                            ItemId = itemid,
                            LikeCount = 1
                        });
                    }
                    else
                    {
                        var itemLikeCount = await _context.ItemLikeCounts.FirstAsync(l => l.ItemId == like.ItemId);
                        itemLikeCount.LikeCount++;
                    }

                    await _context.SaveChangesAsync();

                    var likeCount = await _context.ItemLikeCounts
                        .Where(ilc => ilc.ItemId == itemid)
                        .Select(ilc => ilc.LikeCount) 
                        .FirstOrDefaultAsync();

                    return Json(new { success = true, likeCount });
                }

                return Json(new { success = false, message = "You have already liked this item." });

            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        public async Task<IActionResult>  SearchByTagName(string tagName)
        {
            if(tagName == null)
            {
                return BadRequest("Invalid tag name.");
            }   

            var itemIds = await _context.Tags
                .Where(e => e.Name == tagName)
                .ToListAsync();

            var items = new List<Item>();

            itemIds.ForEach(async e =>
            {
                var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == e.ItemId);
                if (item != null)
                {
                    items.Add(item);
                }
            });

            return View(items);
        }


        public async Task<bool> IsAdmin()
        {
            var sessionUserIdString = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(sessionUserIdString))
            {
                return false;
            }

            var id = Guid.Parse(sessionUserIdString);
            return await _userService.IsAdminAsync(id);
        }

        public async Task<bool> IsValidUserAsync(Guid userid)
        {
            var sessionUserIdString = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(sessionUserIdString))
            {
                return false;
            }
            var sessionUserIdGuid = Guid.Parse(sessionUserIdString);

            var isUserExist = await _userService.GetUserByIdAsync(userid);

            if (sessionUserIdGuid == userid && isUserExist != null)
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