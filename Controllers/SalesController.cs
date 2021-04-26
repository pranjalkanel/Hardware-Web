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
                Console.Out.WriteLine(itemBufferList);
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
                Console.Out.WriteLine(itemBufferList);
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
                    //HttpContext.Session.SetString("ItemBufferList", JsonSerializer.SerializeObject(new ItemBufferList{ List = itemBufferList}));
                    //HttpContext.Session.SetString("ItemBufferList", JsonConvert.SerializeObject(new ItemBufferList { List = itemBufferList}));
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

        public IActionResult Clear()
        {
            var itemList = _context.Items.ToList();
            itemList.Insert(0, new Item { ItemId = 0, Name = "Select" });
            ViewBag.ItemList = itemList;
            if (HttpContext.Session.GetString("ItemBufferList") != null)
            {
                HttpContext.Session.Remove("ItemBufferList");
            }
            return View("~/Views/Sales/Index.cshtml");
        }
    }
}
