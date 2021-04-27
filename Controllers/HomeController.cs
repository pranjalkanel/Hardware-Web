using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HardwareWeb.Models;
using HardwareWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HardwareWeb.DataStores;
using Microsoft.EntityFrameworkCore;

namespace HardwareWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HardwareContext _context;

        public HomeController(ILogger<HomeController> logger, HardwareContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            if ( hasLowItems())
            {
                NotificationStore notification = new NotificationStore();
                notification.MessageText = "Some Items Are Running Low on Stock!";
                notification.ControllerText = "Items";
                notification.ControllerMethodText = "LowStock";
                ViewBag.Message = notification;
            }

            DashViewModel dash = new DashViewModel();

            dash.LastMonthItemsList = LastMonthItems();
            dash.LongStockItemsList = TooLongStockItems();
            dash.CustomerNoSaleList = InactiveCustomers();

            return View(dash);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public bool hasLowItems()
        {
            HardwareContext _context = new HardwareContext();
            var a = _context.Items.Where(x => x.Quantity < 10).ToList();
            

            if (a.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Item> TooLongStockItems()
        {

            string year = DateTime.Now.AddMonths(-6).ToString("yyyy");
            string month = DateTime.Now.AddMonths(-6).ToString("MM");
            string day = DateTime.Now.AddMonths(-6).ToString("dd");
            //return _context.Items.Include(x => x.Stocks.Where(y => y.PurchaseDate < DateTime.Now.AddMonths(-6))).ToList();

            return _context.Items.FromSqlRaw($"SELECT * FROM dbo.items WHERE item_id IN (SELECT item_id FROM dbo.stocks WHERE purchase_date < CAST('{year}{month}{day} 00:00:00.000' AS DATETIME) GROUP BY item_id)").ToList();

        }

        public List<Item> LastMonthItems()
        {
            string year = DateTime.Now.AddDays(-31).ToString("yyyy");
            string month = DateTime.Now.AddDays(-31).ToString("MM");
            string day = DateTime.Now.AddDays(-31).ToString("dd");
            return _context.Items.FromSqlRaw("SELECT * FROM dbo.items WHERE item_id IN" +
                "(SELECT item_id FROM dbo.sale_details WHERE sale_id IN " +
                $"(SELECT sale_id FROM dbo.sales WHERE date < CAST('{year}{month}{day} 00:00:00.000' AS DATETIME)) GROUP BY item_id)").ToList();
        }

        public List<Customer> InactiveCustomers()
        {
            string year = DateTime.Now.AddDays(-31).ToString("yyyy");
            string month = DateTime.Now.AddDays(-31).ToString("MM");
            string day = DateTime.Now.AddDays(-31).ToString("dd");

            return _context.Customers.FromSqlRaw("SELECT * FROM dbo.customers WHERE customer_id " +
                $"IN (SELECT customer_id FROM dbo.sales GROUP BY customer_id HAVING MAX(Date) < CAST('{year}{month}{day} 00:00:00.000' AS datetime)); ").ToList();
        }
    }
}
