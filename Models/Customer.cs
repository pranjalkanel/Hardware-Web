using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("customers")]
    public partial class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }

        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Required]
        [Column("full_name")]
        public string FullName { get; set; }
        [Required]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Column("phone")]
        [StringLength(50)]
        public string Phone { get; set; }
        [Required]
        [Column("address")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column("membership")]
        public int? Membership { get; set; }

        [ForeignKey(nameof(Membership))]
        [InverseProperty("Customers")]
        public virtual Membership MembershipNavigation { get; set; }
        [InverseProperty(nameof(Sale.Customer))]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
