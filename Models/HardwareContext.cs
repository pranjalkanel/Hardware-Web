using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HardwareWeb.Models
{
    public partial class HardwareContext : DbContext
    {
        public HardwareContext()
        {
        }

        public HardwareContext(DbContextOptions<HardwareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleDetail> SaleDetails { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PRANJAL;Initial Catalog=HardwareDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FullName).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.MembershipNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Membership)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_customers_memberships");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_items_item_categories");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.MembershipName).IsUnicode(false);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_sales_customers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_sales_users");
            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_sale_details_items");

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_sale_details_sales");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_stocks_items");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_stocks_users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserGroup).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
