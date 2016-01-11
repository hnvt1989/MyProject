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
    public class DatabaseMasterInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DbContext>
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



            //set up products 
            using (var cContext = new ShoppingCartContext())
            {

                //set up account address

                AddressFlow.AddNewAddress("huynguyenvt1989@gmail.com", new Address()
                {
                    AddressType = new AddressType() { Code = "Primary", Description = "Primary address" },
                    Code = Guid.NewGuid().ToString(),
                    Line1 = "1508 duong 30 thang 4",
                    Line2 = "F12, Tp Vung tau",
                    Line3 = "Viet Nam",
                    Primary = true
                });

                AddressFlow.AddNewAddress("huynguyenvt1989@gmail.com", new Address()
                {
                    AddressType = new AddressType() { Code = "Alternative", Description = "Alternative address" },
                    Code = Guid.NewGuid().ToString(),
                    Line1 = "17000 duong Phan Chau Trinh",
                    Line2 = "F11, Tp Vung tau",
                    Line3 = "Viet Nam",
                    Primary = false
                });

                var sequences = new List<Sequence>
                {
                    new Sequence(){Code = "Order", StartValue = 10000, CurrentValue = 10000},
                    new Sequence(){Code = "Item", StartValue = 20000, CurrentValue = 20004},
                    new Sequence(){Code = "Category", StartValue = 30000, CurrentValue = 30001}
                };

                cContext.Sequences.AddRange(sequences);
                cContext.SaveChanges();

                cContext.Categories.AddRange(new List<Category>
                {
                    new Category {Code = "30000", Description = "Female-Winter-Collection"},
                    new Category {Code = "30001", Description = "Female-Casual-Collection"},
                });
                cContext.SaveChanges();

                var products = new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Code = "20000",
                        Description = "Heathered Knit Drawstring Jumpsuit",
                        Image =
                            ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image1.jpg"))),
                        FeatureProduct = true,
                        Weight = 1,
                        QuantityOnHand = 29,
                        DetailDescription = "this is a good product , Heathered Knit Drawstring Jumpsuit. Ship from USA.",
                        Categories = new List<Category>()
                        {
                            cContext.Categories.SingleOrDefault(c=>c.Code=="30000")
                        },
                        //ProductOffers = new List<ProductOffer>()
                        //{
                        //    new ProductOffer()
                        //    {
                        //        Code = "1001-R",
                        //        Product = cContext.Products.Single(p=> p.Code == "1001"),
                        //        Price = 15m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                        //    },
                        //    new ProductOffer()
                        //    {
                        //        Code = "1001-W",
                        //        Product = cContext.Products.Single(p=> p.Code == "1001"),
                        //        Price = 12m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                        //    }
                        //}
                    },
                    new Product
                    {
                        Id = 2,
                        Code = "20001",
                        Description = "Two-pocket Gingham Shirt",
                        Image =
                            ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image2.jpg"))),
                        FeatureProduct = true,
                        Weight = 0.5m,
                        QuantityOnHand = 22,
                        DetailDescription = "this is a good product , Heathered Knit Drawstring Jumpsuit. Ship from USA.",
                        Categories = new List<Category>()
                        {
                            cContext.Categories.SingleOrDefault(c=>c.Code=="30000")
                        },
                        //ProductOffers = new List<ProductOffer>()
                        //{
                        //    new ProductOffer()
                        //    {
                        //        Code = "1002-R",
                        //        Product = cContext.Products.Single(p=> p.Code == "1002"),
                        //        Price = 12.5m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                        //    },
                        //    new ProductOffer()
                        //    {
                        //        Code = "1002-W",
                        //        Product = cContext.Products.Single(p=> p.Code == "1002"),
                        //        Price = 10.5m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                        //    }
                        //}
                    },
                    new Product
                    {
                        Id = 3,
                        Code = "20002",
                        Description = "Upside-Down Eiffei Tower Tee",
                        Image =
                            ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image3.jpg"))),
                        FeatureProduct = true,
                        Weight = 0.25m,
                        QuantityOnHand = 40,
                        DetailDescription = "this is a good product , Heathered Knit Drawstring Jumpsuit. Ship from USA.",
                        Categories = new List<Category>()
                        {
                            cContext.Categories.SingleOrDefault(c=>c.Code=="30001")
                        },
                        //ProductOffers = new List<ProductOffer>()
                        //{
                        //    new ProductOffer()
                        //    {
                        //        Code = "1003-R",
                        //        Product = cContext.Products.Single(p=> p.Code == "1003"),
                        //        Price = 6.5m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                        //    },
                        //    new ProductOffer()
                        //    {
                        //        Code = "1003-W",
                        //        Product = cContext.Products.Single(p=> p.Code == "1003"),
                        //        Price = 5.5m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                        //    }
                        //}
                    },
                    new Product
                    {
                        Id = 4,
                        Code = "20003",
                        Description = "Perforated Faux Leather Loafers",
                        DetailDescription = "this is a good product , Heathered Knit Drawstring Jumpsuit. Ship from USA.",
                        Image =
                            ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image4.jpg"))),
                        FeatureProduct = true,
                        Weight = .125m,
                        QuantityOnHand = 15,
                        Categories = new List<Category>()
                        {
                            cContext.Categories.SingleOrDefault(c=>c.Code=="30001")
                        },
                        //ProductOffers = new List<ProductOffer>()
                        //{
                        //    new ProductOffer()
                        //    {
                        //        Code = "1004-R",
                        //        Product = cContext.Products.Single(p=> p.Code == "1004"),
                        //        Price = 8.5m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                        //    },
                        //    new ProductOffer()
                        //    {
                        //        Code = "1004-W",
                        //        Product = cContext.Products.Single(p=> p.Code == "1004"),
                        //        Price = 7.25m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                        //    }
                        //}
                    },
                    new Product
                    {
                        Id = 5,
                        Code = "20004",
                        Description = "Faux Leather Skinny Pants",
                        DetailDescription = "this is a good product , Heathered Knit Drawstring Jumpsuit. Ship from USA.",
                        Image =
                            ImageToByteArray(Image.FromFile(HostingEnvironment.MapPath(@"~/Content/Images/Image5.jpg"))),
                        FeatureProduct = false,
                        Weight = 1m,
                        QuantityOnHand = 20,
                        Categories = new List<Category>()
                        {
                            cContext.Categories.SingleOrDefault(c=>c.Code=="30001")
                        },
                        //ProductOffers = new List<ProductOffer>()
                        //{
                        //    new ProductOffer()
                        //    {
                        //        Code = "1005-R",
                        //        Product = cContext.Products.Single(p=> p.Code == "1005"),
                        //        Price = 10m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                        //    },
                        //    new ProductOffer()
                        //    {
                        //        Code = "1005-W",
                        //        Product = cContext.Products.Single(p=> p.Code == "1005"),
                        //        Price = 9m,
                        //        Discountable = true,
                        //        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                        //    }
                        //}
                    }
                };

                products.ForEach(s => cContext.Products.Add(s));
                cContext.SaveChanges();

                cContext.PriceTypes.AddRange(new List<PriceType>()
                {
                    new PriceType()
                    {
                        Code = "R",
                        Description = "Retail",
                    },
                    new PriceType()
                    {
                        Code = "W",
                        Description = "Whole Sale",
                    }
                });
                cContext.SaveChanges();


                cContext.ProductOffers.AddRange(new List<ProductOffer>()
                {
                    new ProductOffer()
                    {
                        Code = "20000-R",
                        Product = cContext.Products.Single(p=> p.Code == "20000"),
                        Price = 300000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                    },
                    new ProductOffer()
                    {
                        Code = "20000-W",
                        Product = cContext.Products.Single(p=> p.Code == "20000"),
                        Price = 240000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                    },
                    new ProductOffer()
                    {
                        Code = "20001-R",
                        Product = cContext.Products.Single(p=> p.Code == "20001"),
                        Price = 200000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                    },
                    new ProductOffer()
                    {
                        Code = "20001-W",
                        Product = cContext.Products.Single(p=> p.Code == "20001"),
                        Price = 180000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                    },
                    new ProductOffer()
                    {
                        Code = "20002-R",
                        Product = cContext.Products.Single(p=> p.Code == "20002"),
                        Price = 310000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                    },
                    new ProductOffer()
                    {
                        Code = "20002-W",
                        Product = cContext.Products.Single(p=> p.Code == "20002"),
                        Price = 280000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                    },
                    new ProductOffer()
                    {
                        Code = "20003-R",
                        Product = cContext.Products.Single(p=> p.Code == "20003"),
                        Price = 500000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                    },
                    new ProductOffer()
                    {
                        Code = "20003-W",
                        Product = cContext.Products.Single(p=> p.Code == "20003"),
                        Price = 450000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                    },
                    new ProductOffer()
                    {
                        Code = "20004-R",
                        Product = cContext.Products.Single(p=> p.Code == "20004"),
                        Price = 430000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "R"),
                    },
                    new ProductOffer()
                    {
                        Code = "20004-W",
                        Product = cContext.Products.Single(p=> p.Code == "20004"),
                        Price = 400000m,
                        Discountable = true,
                        PriceType = cContext.PriceTypes.Single(t=>t.Code == "W"),
                    },
                });

                cContext.SaveChanges();

                //promotion
                cContext.Promotions.AddRange(new List<Promotion>()
                {
                    new Promotion()
                    {
                        Code   = "Christmas discount",
                        PromotionLineItems = new List<PromotionLineItem>()
                        {
                            new PromotionLineItem()
                            {
                                Code = "15PercentOff_1001",
                                Description = "15 percent off Female-Winter-Collection",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "Category=1001;PriceType=R;PercentDiscount=0.15"
                            },
                            new PromotionLineItem()
                            {
                                Code = "15offFemale-Casual-Collection",
                                Description = "15d off Female-Casual-Collection",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "Category=1002;PriceType=R;AmountDiscount=15000"
                            },
                            new PromotionLineItem()
                            {
                                Code = "25_percent_off_item_1004",
                                Description = "25 percent off item 1004",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "ItemCode=1004;PriceType=R;PercentDiscount=0.25"
                            },
                            new PromotionLineItem()
                            {
                                Code = "25_off_item_item_1005",
                                Description = "25 off item item 1005",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "ItemCode=1005;PriceType=R;AmountDiscount=25000"
                            },
                            new PromotionLineItem()
                            {
                                Code = "Buy_5_1004_get_1_item_1004_free",
                                Description = "Buy 5 item 1004 get 1 item 1004 free",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "PriceType=R;BuyItemCode=1004;BuyItemCount=5;GetItemCode=1004;GetItemCount=1"
                            },
                            new PromotionLineItem()
                            {
                                Code = "Buy_5_item_1004_get_2_item_1001_free",
                                Description = "Buy 5 item 1004 get 2 item 1001 free",
                                Active = false,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(2),
                                PromotionLineItemExpression = "PriceType=R;BuyItemCode=1004;BuyItemCount=5;GetItemCode=1001;GetItemCount=2"
                            },
                        }
                    },

                });
                cContext.SaveChanges();

                //cContext.CartLineItems.AddRange(new List<CartLineItem>
                //{
                //    new CartLineItem()
                //    {
                //        Code = Guid.NewGuid().ToString(),
                //        DiscountAmount = 0m,
                //        Quantity = 5,
                //        OriginalPrice = 0m,
                //        ShippingCost = 0m,
                //        DiscountedPrice = 0m,
                //        ProductCode = "1001",
                //        PriceType = "R",
                //        DateCreated = DateTime.Now
                //    },
                //    new CartLineItem()
                //    {
                //        Code = Guid.NewGuid().ToString(),
                //        DiscountAmount = 0m,
                //        Quantity = 5,
                //        OriginalPrice = 0m,
                //        ShippingCost = 0m,
                //        DiscountedPrice = 0m,
                //        ProductCode = "1002",
                //        PriceType = "W",
                //        DateCreated = DateTime.Now
                //    },
                //});

                //cContext.SaveChanges();

                var paymentStatuses = new List<PaymentStatus>
                {
                    new PaymentStatus() { Code = "Hold", Description = "On Hold"},
                    new PaymentStatus() { Code = "Processing", Description = "Processing"},
                    new PaymentStatus() { Code = "Processed", Description = "Processed"},
                    new PaymentStatus() { Code = "Completed", Description = "Completed"}
                };

                cContext.PaymentStatuses.AddRange(paymentStatuses);
                cContext.SaveChanges();


                cContext.PaymentTypes.AddRange(new List<PaymentType>
                {
                    new PaymentType() {Code = "Bank", Description = "Chuyển tiền qua tài khoản"},
                    new PaymentType() {Code = "Cash", Description = "Gởi tiền mặt"},
                });
                cContext.SaveChanges();

                cContext.PaymentTransactions.AddRange(new List<PaymentTransaction>
                {
                    new PaymentTransaction()
                    {
                        Code = Guid.NewGuid().ToString(),
                        Amount = 6.66m,
                        PartialPayment = true,
                        PaymentStatus = cContext.PaymentStatuses.Single(ps => ps.Code == "Completed"),
                        PaymentTypeId = 1,
                        PostedAmount = 6.66m,
                        
                    }
                });


                var paymentTransactionId = cContext.SaveChanges();

                cContext.Carts.AddRange(new List<Cart>
                {
                    new Cart()
                    {
                        Product = cContext.Products.Single(p=> p.Code == "20003"),
                        Code = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        DateCreated = DateTime.Now,
                    }
                });


                cContext.Orders.AddRange(new List<Order>
                {
                    new Order()
                    {
                        OrderNumber = SeqHelper.Next("Order"),
                        UserName = "huynguyenvt1989@gmail.com",
                        FullName = "Jason Nguyen",
                        Phone = "503-111-4444",
                        OrderDate =  DateTime.Now,
                        ShippingAddressId = 1,
                        PaymentTransactionId = paymentTransactionId,
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
                });

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