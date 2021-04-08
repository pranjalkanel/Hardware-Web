using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("sales")]
    public partial class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        [Key]
        [Column("sale_id")]
        public int SaleId { get; set; }
        [Column("customer_id")]
        public int? CustomerId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("date", TypeName = "date")]
        public DateTime Date { get; set; }
        [Column("sale_total")]
        public int SaleTotal { get; set; }
        [Column("discounted")]
        public bool? Discounted { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Sales")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Sales")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(SaleDetail.Sale))]
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
