using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HardwareWeb.Models;
using Microsoft.AspNetCore.Authorization;
using HardwareWeb.DataStores;

namespace HardwareWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View();
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


    }
}
