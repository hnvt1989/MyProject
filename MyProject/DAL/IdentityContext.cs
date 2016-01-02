using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MyProject.Models;
using MyProject.Models.Account;

namespace MyProject.DAL
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {

        public IdentityContext()
            : base("DefaultConnection")
        {
        }

        public static IdentityContext Create()
        {

              return new IdentityContext();
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        //}

        
    }
}