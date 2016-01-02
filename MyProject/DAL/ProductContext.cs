//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Linq;
//using System.Web;
//using MyProject.Models;
//using MyProject.Models.ShoppingCart;

//namespace MyProject.DAL
//{
//    public class ProductContext: DbContext
//    {
//        public ProductContext() : base("DefaultConnection")
//        {
            
//        }

//        public DbSet<Product> Products { get; set; }
//        public DbSet<Category> Categories { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
//        }
//    }
//}