using System.Drawing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyProject.Models.Account;

namespace MyProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration_IdentityDb : DbMigrationsConfiguration<MyProject.DAL.IdentityContext>
    {
        public Configuration_IdentityDb()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyProject.DAL.IdentityContext context)
        {
            if (true)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    Email = "huynguyenvt1989@gmail.com",
                    UserName = "huynguyenvt1989@gmail.com",
                    PhoneNumber = "503-984-5029",
                    FirstName = "Jason",
                    LastName = "Nguyen",
                    //ProfilePicture = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile1.jpg"))),
                };
                userManager.Create(userToInsert, "vt1989");

                userToInsert = new ApplicationUser
                {
                    Email = "muctim308@gmail.com",
                    UserName = "muctim308@gmail.com",
                    FirstName = "Amy",
                    LastName = "Vo",
                    //ProfilePicture = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile2.jpg"))),
                };
                userManager.Create(userToInsert, "vt1989");

                userToInsert = new ApplicationUser
                {
                    Email = "test.user@gmail.com",
                    UserName = "test.user@gmail.com",
                    FirstName = "Test",
                    LastName = "User",
                    //ProfilePicture = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile2.jpg"))),
                };
                userManager.Create(userToInsert, "test1234");
            }
        }

        public byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }
    }
}
