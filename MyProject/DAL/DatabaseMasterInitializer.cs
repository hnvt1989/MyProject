using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyProject.Models;
using MyProject.Models.Account;
using MyProject.Models.ShoppingCart;

namespace MyProject.DAL
{
    public class DatabaseMasterInitializer : System.Data.Entity.DropCreateDatabaseAlways<DbContext>
    {
        protected override void Seed(DbContext context)
        {
            using (var idc = new IdentityContext())
            {
                var userStore = new UserStore<ApplicationUser>(idc);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    Email = "huynguyenvt1989@gmail.com",
                    UserName = "huynguyenvt1989@gmail.com",
                    FirstName = "Jason",
                    LastName = "Nguyen",
                    ProfilePicture = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile1.jpg"))),
                };
                userManager.Create(userToInsert, "vt1989");

                userToInsert = new ApplicationUser
                {
                    Email = "muctim308@gmail.com",
                    UserName = "muctim308@gmail.com",
                    FirstName = "Amy",
                    LastName = "Vo",
                    ProfilePicture = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Profile2.jpg"))),
                };
                userManager.Create(userToInsert, "vt1989");
            }

            //using (var pContext = new ProductContext())
            //{
            //    var categories = new List<Category>
            //{
            //new Category{Id = 101, Description = "Female-Winter-Collection"},
            //new Category{Id = 102, Description = "Female-Casual-Collection"},
            //};
            //    categories.ForEach(s => pContext.Categories.Add(s));
            //    pContext.SaveChanges();

            //    var products = new List<Product>
            //{
            //new Product{Id = 1001, Description = "Heathered Knit Drawstring Jumpsuit", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image1.jpg"))), Id = 101, FeatureProduct = true, Price = 9.99m},
            //new Product{Id = 1002, Description = "Two-pocket Gingham Shirt", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image2.jpg"))), Id = 102, FeatureProduct = true, Price = 11.99m},
            //new Product{Id = 1003, Description = "Upside-Down Eiffei Tower Tee", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image3.jpg"))), Id = 102, FeatureProduct = true, Price = 7.95m},
            //new Product{Id = 1004, Description = "Perforated Faux Leather Loafers", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image4.jpg"))), Id = 101, FeatureProduct = true, Price = 5.95m},
            //new Product{Id = 1005, Description = "Faux Leather Skinny Pants", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image5.jpg"))), Id = 101, FeatureProduct = false, Price = 15.99m}
            //};

            //    products.ForEach(s => pContext.Products.Add(s));
            //    pContext.SaveChanges();
            //}

            using (var cContext = new ShoppingCartContext())
            {

                var categories = new List<Category>
            {
            new Category{Id = 1, Code = "1001", Description = "Female-Winter-Collection"},
            new Category{Id = 2, Code = "1002", Description = "Female-Casual-Collection"},
            };
                categories.ForEach(s => cContext.Categories.Add(s));
                cContext.SaveChanges();

                var products = new List<Product>
            {
            new Product{Id = 1, Code = "1001", Description = "Heathered Knit Drawstring Jumpsuit", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image1.jpg"))), CategoryId = 1, FeatureProduct = true, Price = 9.99m},
            new Product{Id = 2, Code = "1002", Description = "Two-pocket Gingham Shirt", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image2.jpg"))), CategoryId = 2, FeatureProduct = true, Price = 11.99m},
            new Product{Id = 3, Code = "1003", Description = "Upside-Down Eiffei Tower Tee", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image3.jpg"))), CategoryId = 1, FeatureProduct = true, Price = 7.95m},
            new Product{Id = 4, Code = "1004", Description = "Perforated Faux Leather Loafers", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image4.jpg"))), CategoryId = 1, FeatureProduct = true, Price = 5.95m},
            new Product{Id = 5, Code = "1005", Description = "Faux Leather Skinny Pants", Image = ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image5.jpg"))), CategoryId = 1, FeatureProduct = false, Price = 15.99m}
            };
                products.ForEach(s => cContext.Products.Add(s));

                var carts = new List<Cart>
                {
                    new Cart()
                    {
                        ProductId = 1,
                        Code = Guid.NewGuid().ToString(),
                        Count = 1,
                        DateCreated = DateTime.Now,
                    }
                };

                var orders = new List<Order>
                {
                    new Order()
                    {
                        UserName = "huynguyenvt1989@gmail.com",
                        FirstName = "Jason",
                        LastName = "Nguyen",
                        Phone = "503-111-4444",
                        OrderDate =  DateTime.Now
                    }
                };
                var lineOrderDetails = new List<LineOrderDetail>
                {
                    new LineOrderDetail
                    {
                        ProductId = 1,
                        OrderId = 0,
                        UnitPrice = 9.99m,
                        Quantity = 1
                    }
                };

                //carts.ForEach(c => cContext.Carts.Add(c));
                cContext.Carts.AddRange(carts);
                cContext.Orders.AddRange(orders);
                cContext.LineOrderDetails.AddRange(lineOrderDetails);
                cContext.SaveChanges();

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