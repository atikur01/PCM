using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;
using System;

namespace PCM.Controllers
{
    public class CollectionController : Controller
    {
        private readonly AppDbContext _context;

        public CollectionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var collections = _context.Collections.Include(c => c.Items).ToList();
            return View(collections);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Collection collection)
        {
            
                _context.Collections.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
          //  return View(collection);
        }

        public IActionResult Details(int id)
        {
            var collection = _context.Collections.Include(c => c.Items).FirstOrDefault(c => c.Id == id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }
    }
}
