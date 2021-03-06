using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace HardwareWeb.Controllers
{
    public class ItemsController : Controller
    {
        private readonly HardwareContext _context;

        public ItemsController(HardwareContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Items
        public async Task<IActionResult> Index()
        {
            var hardwareContext = _context.Items.Include(i => i.CategoryNavigation).Include(x => x.Stocks).OrderBy(q => q.Quantity);
            return View(await hardwareContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize]
        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.ItemCategories, "CategoryId", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Name,Description,Price,Quantity,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.ItemCategories, "CategoryId", "Name", item.Category);
            return View(item);
        }

        [Authorize]
        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.ItemCategories, "CategoryId", "Name", item.Category);
            return View(item);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,Description,Price,Quantity,Category")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.ItemCategories, "CategoryId", "Name", item.Category);
            return View(item);
        }

        [Authorize(Policy = "AdminRolePolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize(Policy = "AdminRolePolicy")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> LowStock()
        {
            var lowItems = _context.Items.Where(x => x.Quantity<10).Include(i => i.CategoryNavigation);
            return View(await lowItems.ToListAsync());
        }
            private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }

        public async Task<IActionResult> SortBy(int sortOption)
        {
            var obj = _context.Items.Include(i => i.CategoryNavigation).Include(x => x.Stocks).OrderBy(q => q.Quantity);
            if (sortOption == 0)
            {
                return View("~/Views/Items/Index.cshtml", await obj.ToListAsync());
            }
            else if (sortOption == 1)
            {
                obj = _context.Items.Include(i => i.CategoryNavigation).Include(x => x.Stocks).OrderBy(q => q.Name);
            }
            else if (sortOption == 2)
            {
               obj = _context.Items.Include(i => i.CategoryNavigation).Include(x => x.Stocks).OrderByDescending(q => q.Quantity);
            }
            else
            {
                obj = _context.Items.Include(i => i.CategoryNavigation).Include(x => x.Stocks).OrderByDescending(q => q.Stocks.FirstOrDefault().PurchaseDate);
            }
            return View("~/Views/Items/Index.cshtml", await obj.ToListAsync());
        }
    }
}
