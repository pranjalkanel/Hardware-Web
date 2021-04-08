using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("stocks")]
    public partial class Stock
    {
        [Key]
        [Column("stock_id")]
        public int StockId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("item_id")]
        public int? ItemId { get; set; }
        [Column("purchase_date", TypeName = "date")]
        public DateTime PurchaseDate { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(ItemId))]
        [InverseProperty("Stocks")]
        public virtual Item Item { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Stocks")]
        public virtual User User { get; set; }
    }
}
