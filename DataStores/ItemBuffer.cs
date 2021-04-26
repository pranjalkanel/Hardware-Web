using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareWeb.DataStores
{
    public class ItemBuffer
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int SaleAmount { get; set; }
        public int ItemRate { get; set; }
        public int LineTotal { get; set; }
    }
}
