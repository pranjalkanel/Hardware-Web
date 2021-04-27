using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareWeb.ViewModels
{
    public class NewSaleLineViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Select an Item to Add")]
        public int ItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sale Unit should be greater than 0")]
        public int ItemAmount { get; set; }
    }
}
