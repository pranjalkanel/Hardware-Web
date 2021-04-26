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
        public int ItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int ItemAmount { get; set; }
    }
}
