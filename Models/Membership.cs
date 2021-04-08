using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("memberships")]
    public partial class Membership
    {
        public Membership()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [Column("membership_id")]
        public int MembershipId { get; set; }
        [Required]
        [Column("membership_name")]
        [StringLength(50)]
        public string MembershipName { get; set; }
        [Column("description")]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("discount")]
        public int Discount { get; set; }

        [InverseProperty(nameof(Customer.MembershipNavigation))]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
