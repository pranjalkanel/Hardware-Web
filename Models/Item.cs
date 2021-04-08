using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("items")]
    public partial class Item
    {
        public Item()
        {
            SaleDetails = new HashSet<SaleDetail>();
            Stocks = new HashSet<Stock>();
        }

        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public int Price { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("category")]
        public int? Category { get; set; }

        [ForeignKey(nameof(Category))]
        [InverseProperty(nameof(ItemCategory.Items))]
        public virtual ItemCategory CategoryNavigation { get; set; }
        [InverseProperty(nameof(SaleDetail.Item))]
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        [InverseProperty(nameof(Stock.Item))]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
