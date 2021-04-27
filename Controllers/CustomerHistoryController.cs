using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareWeb.Models;
using Microsoft.EntityFrameworkCore;
using HardwareWeb.ViewModels;
namespace HardwareWeb.Controllers
{
    public class CustomerHistoryController : Controller
    {
        private readonly HardwareContext _context;

        public CustomerHistoryController(HardwareContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var customerList = _context.Customers.ToList();
            customerList.Insert(0, new Customer { CustomerId = 0, FullName = "Select" });
            ViewBag.CustomerList = customerList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int CustomerId)
        {

            var customerList = _context.Customers.ToList();
            customerList.Insert(0, new Customer { CustomerId = 0, FullName = "Select" });
            ViewBag.CustomerList = customerList;

            if (CustomerId > 0)
            {
                   
                var sales = _context.Sales
                    .Where(sale => sale.CustomerId == CustomerId && sale.Date.Date >= (DateTime.Now.AddDays(-31))).OrderBy(sale => sale.Date);

                if (sales.Any())
                {

                    var saleDetails1 = (from detail in _context.SaleDetails 
                                      join item in _context.Items
                                      on detail.ItemId equals item.ItemId
                                      select new {
                                          sale_id = detail.SaleId,
                                        item_name = item.Name,
                                        order_unit = detail.OrderUnit,
                                        line_total = detail.LineTotal,
                                        item_price = item.Price
                                      }).AsQueryable();

                    var saleDetails2 = sales.Join(saleDetails1, x => x.SaleId, y => y.sale_id, (sales,saleDetails1) => new SaleDetailsViewModel(){ 
                        SaleId= saleDetails1.sale_id ?? default(int), 
                        ItemName = saleDetails1.item_name, 
                        OrderUnit = saleDetails1.order_unit, 
                        LineTotal = saleDetails1.line_total,
                        ItemPrice = saleDetails1.item_price});

                    var saleDataQ = (from sale in _context.Sales
                                     join user in _context.Users
                                     on sale.UserId equals user.UserId
                                     join customer in _context.Customers
                                     on sale.CustomerId equals customer.CustomerId
                                     select new
                                     {
                                         SaleId = sale.SaleId,
                                         SaleDate = sale.Date,
                                         SaleTotal = sale.SaleTotal,
                                         User = user.Name,
                                         Customer = customer.FullName
                                     }
                                     ).AsQueryable();

                    var saleData = sales.Join(saleDataQ, x => x.SaleId, y => y.SaleId, (sales, saleDataQ) => new SaleDataViewModel() {
                        SaleId = saleDataQ.SaleId,
                        SaleDate = saleDataQ.SaleDate,
                        SaleTotal = saleDataQ.SaleTotal,
                        User = saleDataQ.User,
                        Customer = saleDataQ.Customer
                    });

                    ViewBag.Sales = saleData;
                    ViewBag.SaleDetails = saleDetails2;
                }
                else
                {
                    ViewBag.NoData = "No Purchases Made By This Customer In Past Month";
                }
            }
            return View();
        }
    }
}
