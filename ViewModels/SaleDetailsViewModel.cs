using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardwareWeb.Models;

namespace HardwareWeb.ViewModels
{
    public class SaleDetailsViewModel
    {
        public int SaleId { get; set; }

        public string ItemName { get; set; }

        public int ItemPrice { get; set; }

        public int OrderUnit { get; set; }

        public int LineTotal { get; set; }
    }
}
