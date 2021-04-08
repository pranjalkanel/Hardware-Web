using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("sale_details")]
    public partial class SaleDetail
    {
        [Key]
        [Column("line_id")]
        public int LineId { get; set; }
        [Column("item_id")]
        public int? ItemId { get; set; }
        [Column("sale_id")]
        public int? SaleId { get; set; }
        [Column("order_unit")]
        public int OrderUnit { get; set; }
        [Column("line_total")]
        public int LineTotal { get; set; }

        [ForeignKey(nameof(ItemId))]
        [InverseProperty("SaleDetails")]
        public virtual Item Item { get; set; }
        [ForeignKey(nameof(SaleId))]
        [InverseProperty("SaleDetails")]
        public virtual Sale Sale { get; set; }
    }
}
