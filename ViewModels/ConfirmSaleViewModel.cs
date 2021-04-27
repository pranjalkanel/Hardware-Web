using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardwareWeb.DataStores;
using System.ComponentModel.DataAnnotations;

namespace HardwareWeb.ViewModels
{
    public class ConfirmSaleViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Customer for Billing")]
        public int CustomerId { get; set; }
        public List<ItemBuffer> ItemList { get; set; }

        public int SaleTotal { get; set; }
    }
}
