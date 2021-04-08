using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HardwareWeb.Models
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            Sales = new HashSet<Sale>();
            Stocks = new HashSet<Stock>();
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [Column("user_group")]
        [StringLength(50)]
        public string UserGroup { get; set; }

        [InverseProperty(nameof(Sale.User))]
        public virtual ICollection<Sale> Sales { get; set; }
        [InverseProperty(nameof(Stock.User))]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
