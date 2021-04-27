using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HardwareWeb.ViewModels;
using HardwareWeb.DataStores;
using HardwareWeb.SessionExtensions;
using Microsoft.AspNetCore.Http;
//using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace HardwareWeb.Controllers
{
    public class SalesController : Controller
    {
        private readonly HardwareContext _context;

        public SalesController(HardwareContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var itemList = _context.Items.ToList();
            itemList.Insert(0, new Item { ItemId = 0, Name = "Select" });
            ViewBag.ItemList = itemList;

            List<ItemBuffer> itemBufferList = new List<ItemBuffer>();

            if (HttpContext.Session.GetString("ItemBufferList") != null)
            {
                //itemBufferList = JsonSerializer.Deserialize<ItemBufferList>(HttpContext.Session.Get("ItemBufferList")).List;
                var bufferListTmp = HttpContext.Session.GetObjectFromJson<ItemBufferList>("ItemBufferList");
                itemBufferList = bufferListTmp.List;
                ViewBag.ItemBufferList = itemBufferList;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(NewSaleLineViewModel obj)
        {
            var itemList = _context.Items.Where(c => c.Quantity>0).ToList();
            itemList.Insert(0, new Item { ItemId = 0, Name = "Select" });
            ViewBag.ItemList = itemList;


            List<ItemBuffer> itemBufferList = new List<ItemBuffer>();

            if (HttpContext.Session.GetString("ItemBufferList") != null)
            {
                //itemBufferList = JsonSerializer.Deserialize<ItemBufferList>(HttpContext.Session.Get("ItemBufferList")).List;
                var bufferListTmp = HttpContext.Session.GetObjectFromJson<ItemBufferList>("ItemBufferList");
                itemBufferList = bufferListTmp.List;
                ViewBag.ItemBufferList = itemBufferList;
            }
            

            if (ModelState.IsValid)
            {
                var item = _context.Items.Where(item => item.ItemId == obj.ItemId).FirstOrDefault();
                int stock = item.Quantity;
                if (obj.ItemAmount < stock){
                    ItemBuffer itemBuffer = new ItemBuffer();
                    itemBuffer.ItemId = obj.ItemId;
                    itemBuffer.ItemName = item.Name;
                    itemBuffer.ItemRate = item.Price;
                    itemBuffer.SaleAmount = obj.ItemAmount;
                    itemBuffer.LineTotal = (item.Price * obj.ItemAmount);

                    itemBufferList.Add(itemBuffer);

                    ItemBufferList listObj = new ItemBufferList();
                    listObj.List = itemBufferList;
                    HttpContext.Session.SetObjectAsJson("ItemBufferList",listObj);
                    ViewBag.ItemBufferList = itemBufferList;
                }
                else
                {
                    ViewBag.StockMessage = "Entered amount not in stock";
                }
                
            }
            return View();
        }

        public void clearSessionItems()
        {
            var itemList = _context.Items.ToList();
            itemList.Insert(0, new Item { ItemId = 0, Name = "Select" });
            ViewBag.ItemList = itemList;
            if (HttpContext.Session.GetString("ItemBufferList") != null)
            {
                HttpContext.Session.Remove("ItemBufferList");
            }
        }

        public IActionResult Clear()
        {
            clearSessionItems();
            return View("~/Views/Sales/Index.cshtml");
        }

        [HttpGet]
        public IActionResult Confirm()
        {
            ConfirmSaleViewModel obj = getObj();

            var customerList = _context.Customers.ToList();
            customerList.Insert(0, new Customer { CustomerId = 0, FullName = "Select" });
            ViewBag.CustomerList = customerList;

            

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult  Confirm(ConfirmSaleViewModel inputobj, int UserId)
        {
            if (ModelState.IsValid)
            {
                Sale sale = new Sale();
                sale.CustomerId = inputobj.CustomerId;
                sale.UserId = UserId;
                sale.SaleTotal = inputobj.SaleTotal;
                sale.Date = DateTime.Now;
                sale.Discounted = false;

                _context.Add(sale);

                _context.SaveChanges();

                List<ItemBuffer> saleItems = getObj().ItemList;

                int saleId = 0;

                using (HardwareContext _saleContext = new HardwareContext())
                {
                    saleId = _saleContext.Sales
                                  .OrderByDescending(x => x.Date)
                                  .FirstOrDefault().SaleId;
                }
                    

                foreach (var item in saleItems)
                {
                    using (HardwareContext _itemContext = new HardwareContext() )
                    {
                        _itemContext.Add(new SaleDetail() { ItemId = item.ItemId, SaleId = saleId, OrderUnit = item.SaleAmount, LineTotal = item.LineTotal });
                        _itemContext.SaveChanges();
                    }
                    
                }
                clearSessionItems();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var customerList = _context.Customers.ToList();
                customerList.Insert(0, new Customer { CustomerId = 0, FullName = "Select" });
                ViewBag.CustomerList = customerList;

                ConfirmSaleViewModel viewobj = getObj();
                return View(viewobj);
            }
        }
    
        public ConfirmSaleViewModel getObj()
        {
            int saleTotal = 0;

            ConfirmSaleViewModel obj = new ConfirmSaleViewModel();

            List<ItemBuffer> itemBufferList;
            var bufferListTmp = HttpContext.Session.GetObjectFromJson<ItemBufferList>("ItemBufferList");
            itemBufferList = bufferListTmp.List;

            foreach (var line in itemBufferList)
            {
                saleTotal = saleTotal + line.LineTotal;
            }

            obj.SaleTotal = saleTotal;
            obj.CustomerId = 0;
            obj.ItemList = itemBufferList;

            return obj;
        }
    }
}
