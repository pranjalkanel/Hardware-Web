using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardwareWeb.Models;

namespace HardwareWeb.ViewModels
{
    public class DashViewModel
    {
        public List<Item> LongStockItemsList {get;set;}

        public List<Customer> CustomerNoSaleList { get; set; }

        public List<Item>  LastMonthItemsList { get; set; }

    }
}
