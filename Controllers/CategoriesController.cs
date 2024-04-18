using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WealthTrackr.Areas.Data;
using WealthTrackr.Data;
using WealthTrackr.Models;
using WealthTrackr.ViewModels;

namespace WealthTrackr.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                // current user 
                var currentUser = await _userManager.GetUserAsync(User);
                Category newCategory = new Category
                {
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    Type = model.Type,
                    FkAccountId = currentUser.Id
                };

                _context.Add(newCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyCategories));
            }
            else
            {
                ViewBag.Error = "Error, please try again later.";
                return View();
            }
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    Category editCategory = new Category
                    { 
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                        Type = category.Type,
                        FkAccountId = currentUser.Id
                    };

                    _context.Update(editCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else

                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyCategories));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        // categories specific to user 
        public async Task<IActionResult> MyCategories()
        {
            // current user 
            var currentUser = await _userManager.GetUserAsync(User);
            // list of all transactions 
            var categories = await _context.Categories.ToListAsync();
            // list of transactions specific to user 

            List<Category> categoriesByUser = [];

            // iterate through list of all transactions 
            foreach (var category in categories)
            {
                if (currentUser.Id == category.FkAccountId)
                {
                    categoriesByUser.Add(category);
                }
            }



            return View(categoriesByUser);
        }

    }
}
