using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyProject.DataContext;
using MyProject.DAL;
using MyProject.Models;

namespace MyProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CustomizeIt.DataContext.IdentityDb";
        }

        protected override void Seed(IdentityContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            //TODO: seed database , Use update-database from package-console
            //if (!(context.Users.Any(u => u.UserName == "huynguyenvt1989@gmail.com")))
            //{
            //    var userStore = new UserStore<ApplicationUser>(context);
            //    var userManager = new UserManager<ApplicationUser>(userStore);
            //    var userToInsert = new ApplicationUser
            //    {
            //        Email = "huynguyenvt1989@gmail.com",
            //        UserName = "huynguyenvt1989@gmail.com",
            //        FullName = "Jason",
            //        LastName = "Nguyen",
            //        ProfilePicture = ProductInitializer.ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile1.jpg"))),
            //    };
            //    userManager.Create(userToInsert, "vt1989");
            //}
        }
    }
}
