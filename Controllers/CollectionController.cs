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
        public async Task<IActionResult> Create(Collection collection, string[] CustomFieldNames, string[] CustomFieldTypes)
        {
            if (ModelState.IsValid)
            {
                // Logic to add custom fields to collection
                for (int i = 0; i < CustomFieldNames.Length; i++)
                {
                    var fieldName = CustomFieldNames[i];
                    var fieldType = CustomFieldTypes[i];

                    switch (fieldType)
                    {
                        case "String":
                            if (collection.CustomString1Name == null) collection.CustomString1Name = fieldName;
                            else if (collection.CustomString2Name == null) collection.CustomString2Name = fieldName;
                            else collection.CustomString3Name = fieldName;
                            break;
                        case "Integer":
                            if (collection.CustomInt1Name == null) collection.CustomInt1Name = fieldName;
                            else if (collection.CustomInt2Name == null) collection.CustomInt2Name = fieldName;
                            else collection.CustomInt3Name = fieldName;
                            break;
                        case "MultilineText":
                            if (collection.CustomMultilineText1Name == null) collection.CustomMultilineText1Name = fieldName;
                            else if (collection.CustomMultilineText2Name == null) collection.CustomMultilineText2Name = fieldName;
                            else collection.CustomMultilineText3Name = fieldName;
                            break;
                        case "Boolean":
                            if (collection.CustomBoolean1Name == null) collection.CustomBoolean1Name = fieldName;
                            else if (collection.CustomBoolean2Name == null) collection.CustomBoolean2Name = fieldName;
                            else collection.CustomBoolean3Name = fieldName;
                            break;
                        case "Date":
                            if (collection.CustomDate1Name == null) collection.CustomDate1Name = fieldName;
                            else if (collection.CustomDate2Name == null) collection.CustomDate2Name = fieldName;
                            else collection.CustomDate3Name = fieldName;
                            break;
                    }
                }

                _context.Collections.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
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

        [HttpGet]
        public IActionResult GetCustomFieldTemplate(string fieldName, string fieldType)
        {
            string template = string.Empty;
            switch (fieldType)
            {
                case "String":
                    template = $@"
                    <div class='form-group'>
                        <label>{fieldName}</label>
                        <input name='{fieldName}' class='form-control' />
                    </div>";
                    break;
                case "Integer":
                    template = $@"
                    <div class='form-group'>
                        <label>{fieldName}</label>
                        <input name='{fieldName}' type='number' class='form-control' />
                    </div>";
                    break;
                case "MultilineText":
                    template = $@"
                    <div class='form-group'>
                        <label>{fieldName}</label>
                        <textarea name='{fieldName}' class='form-control'></textarea>
                    </div>";
                    break;
                case "Boolean":
                    template = $@"
                    <div class='form-group'>
                        <label>{fieldName}</label>
                        <input name='{fieldName}' type='checkbox' class='form-check-input' />
                    </div>";
                    break;
                case "Date":
                    template = $@"
                    <div class='form-group'>
                        <label>{fieldName}</label>
                        <input name='{fieldName}' type='date' class='form-control' />
                    </div>";
                    break;
            }

            return Content(template);
        }


    }
}
