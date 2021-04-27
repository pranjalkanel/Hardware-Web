using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareWeb.Models;
using Newtonsoft.Json;
using HardwareWeb.SessionExtensions;

namespace HardwareWeb.Controllers
{
    public class SaleConfirmController : Controller
    {
        private readonly HardwareContext _context;

        public SaleConfirmController(HardwareContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var customerList = _context.Customers.ToList();
            customerList.Insert(0, new Customer { CustomerId = 0, FullName = "Select" });
            return View();
        }

        [HttpPost]
        public IActionResult Index(int cusId)
        {
            return View();
        }
    }
}
