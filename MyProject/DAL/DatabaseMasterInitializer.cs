using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyProject.Models;
using MyProject.Models.Account;
using MyProject.Models.Core;
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

            using (var cContext = new ShoppingCartContext())
            {
                var sequences = new List<Sequence>
                {
                    new Sequence(){Code = "Order", StartValue = 1000, CurrentValue = 1000}
                };

                cContext.Sequences.AddRange(sequences);
                cContext.SaveChanges();

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
                cContext.SaveChanges();


                var paymentStatuses = new List<PaymentStatus>
                {
                    new PaymentStatus() { Code = "Hold", Description = "On Hold"},
                    new PaymentStatus() { Code = "Processing", Description = "Processing"},
                    new PaymentStatus() { Code = "Processed", Description = "Processed"},
                    new PaymentStatus() { Code = "Completed", Description = "Completed"}
                };

                cContext.PaymentStatuses.AddRange(paymentStatuses);
                cContext.SaveChanges();

                var addresses = new List<Address>
                {
                    new Address(){ Code = Guid.NewGuid().ToString(),Line1 = "1508/4 duong 30 thang 4", Line2 = "Phuong 12, tp Vung Tau", Line3 = "Vietnam"},

                };
                cContext.Addresses.AddRange(addresses);
                cContext.SaveChanges();

                var paymentTypes = new List<PaymentType>
                {
                    new PaymentType() {Code = "Cash", Description = "Cash payment"},
                    new PaymentType() {Code = "PayPal", Description = "PayPal payment"}
                };

                cContext.PaymentTypes.AddRange(paymentTypes);
                cContext.SaveChanges();

                var paymentTransactions = new List<PaymentTransaction>
                {
                    new PaymentTransaction()
                    {
                        Code = Guid.NewGuid().ToString(),
                        Amount = 6.66m,
                        PartialPayment = true,
                        PaymentStatusId = 1,
                        PaymentTypeId = 1,
                        PostedAmount = 6.66m,
                        
                    }
                };
                cContext.PaymentTransactions.AddRange(paymentTransactions);
                cContext.SaveChanges();

                var orders = new List<Order>
                {
                    new Order()
                    {
                        OrderNumber = SeqHelper.Next("Order"),
                        UserName = "huynguyenvt1989@gmail.com",
                        FullName = "Jason Nguyen",
                        Phone = "503-111-4444",
                        OrderDate =  DateTime.Now,
                        ShippingAddressId = 1,
                        PaymentTransactionId = 1,
                        Email = "huynguyenvt1989@gmail.com",
                        OrderDetails = new List<LineOrderDetail>()
                        {
                             new LineOrderDetail
                                {
                                    ProductId = 1,
                                    UnitPrice = 9.99m,
                                    Quantity = 1
                                }
                        }
                    }
                };
                //var lineOrderDetails = new List<LineOrderDetail>
                //{
                //    new LineOrderDetail
                //    {
                //        ProductId = 1,
                //        OrderId = 1,
                //        UnitPrice = 9.99m,
                //        Quantity = 1
                //    }
                //};

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



                cContext.Carts.AddRange(carts);
                cContext.Orders.AddRange(orders);
                //cContext.LineOrderDetails.AddRange(lineOrderDetails);
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