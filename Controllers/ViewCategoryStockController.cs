using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HardwareWeb.Models;

namespace HardwareWeb.Controllers
{
    public class ViewCategoryStockController : Controller
    {
        private readonly HardwareContext _context;

        public ViewCategoryStockController(HardwareContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //List<ItemCategory> categoryList = new List<ItemCategory>();

            var categoryList = _context.ItemCategories.ToList();
            categoryList.Insert(0, new ItemCategory { CategoryId = 0, Name = "Select" });
            ViewBag.ListofCategory = categoryList;
            //if (category > 0)
            //{
                
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int CategoryId)
        {
            var categoryList = _context.ItemCategories.ToList();
            categoryList.Insert(0, new ItemCategory { CategoryId = 0, Name = "Select" });
            ViewBag.ListofCategory = categoryList;
            
            if (CategoryId > 0)
            {
                var categoryItemStock = _context.Items
                    .Where(item => item.Category == CategoryId && item.Quantity > 10);

                if (!categoryItemStock.Any())
                {
                    ViewBag.NoData = "No Item in Stock for Selected Category";
                }
                else
                {
                    ViewBag.SearchResults = categoryItemStock;
                }
                
            }
            return View();
        }
    }
}
