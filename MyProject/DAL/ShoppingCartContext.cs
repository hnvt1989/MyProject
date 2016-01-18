using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web;
using MyProject.Models.Account;
using MyProject.Models.Content;
using MyProject.Models.Core;
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
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AccountAddress> AccountAddresses { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<Sequence> Sequences { get; set; }

        public DbSet<LineOrderDetail> LineOrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductOffer> ProductOffers { get; set; }
        public DbSet<PriceType> PriceTypes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionLineItem> PromotionLineItems { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        //public DbSet<CartLineItem> CartLineItems { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}