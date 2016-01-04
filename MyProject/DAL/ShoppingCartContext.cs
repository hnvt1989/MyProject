using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;

namespace MyProject.DAL
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext() : base("DefaultConnection")
        {
            
        }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<LineOrderDetail> LineOrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}