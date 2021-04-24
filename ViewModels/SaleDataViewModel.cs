using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareWeb.ViewModels
{
    public class SaleDataViewModel
    {
        public int SaleId { get; set; }

        public int SaleTotal { get; set;}

        public DateTime SaleDate { get; set; }

        public string Customer { get; set; }

        public string User { get; set; }
    }
}
