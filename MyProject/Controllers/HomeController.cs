using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var homeView = new HomeViewModel();

            List<ProductViewModel> nProducts = new List<ProductViewModel>();
            List<ProductViewModel> bestSellingProds = new List<ProductViewModel>();

            using (var pContext = new ShoppingCartContext())
            {
                //new products
                var newProds = pContext.Products.Where(p => p.FeatureProduct && p.Active).OrderByDescending(p => p.Id).Take(12);
                var offers = pContext.ProductOffers.ToList();
                foreach (var p in newProds)
                {
                    nProducts.Add(new ProductViewModel()
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Description = p.Description,
                        Price = offers.Single(po => po.ProductId == p.Id && po.PriceTypeId == 1).Price,
                        FeatureProduct = p.FeatureProduct,
                        Image = p.Image
                    });
                }

                foreach (var prod in nProducts)
                {
                    var origPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 3);
                    prod.OriginalPrice = origPrice != null ? origPrice.Price : 0m;
                }

                //best selling products
                //new products
                var startDate = new DateTime(DateTime.Now.Year, 1, 1);
                var endDate = startDate.AddYears(1).AddDays(-1);

                var ordersThisYears = pContext.Orders.Where(o => o.OrderDate <= endDate && o.OrderDate >= startDate && o.OrderStatusId != 7).Select(p => p.Id).ToList();

                var bProds =
                    pContext.LineOrderDetails.Where(o => ordersThisYears.Contains(o.OrderId))
                        .OrderByDescending(ol => ol.Quantity)
                        .Take(8).ToList();
                bProds.ForEach(p => bestSellingProds.Add(new ProductViewModel()
                {
                    Id = p.Product.Id,
                    Code = p.Product.Code,
                    Description = p.Product.Description,
                    Price = offers.Single(po => po.ProductId == p.Product.Id && po.PriceTypeId == 1).Price,
                    FeatureProduct = p.Product.FeatureProduct,
                    Image = p.Product.Image,
                    BoughtQuantity = p.Quantity
                }));
                        //.Select(p => p.Product).Distinct()
                        //.ToList();

                //bestSellingProds.AddRange(bProds.Select(p => new ProductViewModel()
                //{
                //    Id = p.Id, Code = p.Code, Description = p.Description, Price = offers.Single(po => po.ProductId == p.Id && po.PriceTypeId == 1).Price, FeatureProduct = p.FeatureProduct, Image = p.Image
                //}));

                foreach (var prod in bestSellingProds)
                {
                    var origPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 3);
                    prod.OriginalPrice = origPrice != null ? origPrice.Price : 0m;
                    //prod.Price = offers.Single(po => po.ProductId == prod.Id && po.PriceTypeId == 1).Price;
                }
                homeView.BestSellingProductsThisMonths = bestSellingProds;

                var ret = new HeaderAdvertisementViewModel();
                var id = pContext.ContentTypes.Single(p => p.Code == "Ad").Id;

                pContext.Contents.Where(c => c.ContentTypeId == id).ForEach(c => ret.Ads.Add(new HeaderAd()
                {
                    Image = c.Image,
                    Url = c.ImageUrl,
                    DisplayOrder = c.DisplayOrder,
                    AdText = c.AdText,
                    AdTextStyle = c.AdTextStyle

                }));
                ret.Ads = ret.Ads.OrderBy(o => o.DisplayOrder).ToList();

                //ret.Ads.Sort((x,y) => x.DisplayOrder.CompareTo(y.DisplayOrder));

                var contactInfo = pContext.Contents.SingleOrDefault(c => c.TextLocation == "Home.ContactInfo");

                if (contactInfo != null)
                {
                    homeView.Info.ContactInfo = contactInfo.TextValue;
                }

                var annoucement = pContext.Contents.SingleOrDefault(c => c.TextLocation == "Home.Announcement");
                if (annoucement != null)
                {
                    homeView.Info.Annoucment = annoucement.TextValue;
                }

                homeView.Advertisement = ret;

            }

            homeView.NewProducts = nProducts;

            return View(homeView);
        }

        //public ActionResult RenderHeaderAdvertisement()
        //{
        //    var ret = new HeaderAdvertisementViewModel();
        //    using (var context = new ShoppingCartContext())
        //    {
        //        var id = context.ContentTypes.Single(p => p.Code == "Ad").Id;

        //        context.Contents.Where( c => c.ContentTypeId == id).ForEach( c=> ret.Ads.Add(new HeaderAd()
        //        {
        //            Image = c.Image,
        //            Url = c.ImageUrl
        //        }));
        //    }
        //    ret.Ads = ret.Ads.OrderBy(o => o.DisplayOrder).ToList();
        //    return View(ret);
        //}

        public ActionResult FilterByCategory(string code, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                
                if (code == "All")
                    return RedirectToAction("Index");
                var prods = new List<Product>();

                using (var context = new ShoppingCartContext())
                {
                    prods = (from p in context.Products
                        where p.Categories.Any(c => c.Code == code)
                        select p).Where(p=> p.Active).Take(48).OrderByDescending(o => o.Id).ToList();
                }
                using (var context = new ShoppingCartContext())
                {
                    var shippingRate = Convert.ToDecimal(context.AppSettings.Single(a => a.Code == "ShippingRate").Value);
                    var products = new List<ProductViewModel>();
                    var offers = context.ProductOffers.ToList();
                    foreach (var p in prods)
                    {
                        products.Add(new ProductViewModel()
                        {
                            Id = p.Id,
                            Code = p.Code,
                            Description = p.Description,
                            Price = offers.Single(po => po.ProductId == p.Id && po.PriceTypeId == 1).Price + (shippingRate * p.Weight),
                            FeatureProduct = p.FeatureProduct,
                            Image = p.Image
                        });
                    }

                    foreach (var prod in products)
                    {
                        var origPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 3);
                        prod.OriginalPrice = origPrice != null ? origPrice.Price : 0m;
                    }

                    var homeView = new HomeViewModel {FilteredProducts = products};
                    homeView.SelectedCategory = code;

                    //var ret = new HeaderAdvertisementViewModel();
                    //var id = context.ContentTypes.Single(p => p.Code == "Ad").Id;

                    //context.Contents.Where(c => c.ContentTypeId == id).ForEach(c => ret.Ads.Add(new HeaderAd()
                    //{
                    //    Image = c.Image,
                    //    Url = c.ImageUrl,
                    //    AdText = c.AdText,
                    //    AdTextStyle = c.AdTextStyle
                    //}));
                    //ret.Ads = ret.Ads.OrderBy(o => o.DisplayOrder).ToList();

                    //homeView.Advertisement = ret;
                    homeView.Advertisement = new HeaderAdvertisementViewModel()
                    {
                        Ads = new List<HeaderAd>()
                    };

                    var contactInfo = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.ContactInfo");

                    if (contactInfo != null)
                    {
                        homeView.Info.ContactInfo = contactInfo.TextValue;
                    }

                    var annoucement = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.Announcement");
                    if (annoucement != null)
                    {
                        homeView.Info.Annoucment = annoucement.TextValue;
                    }

                    ViewBag.ProductHeader = string.Empty;
                    return View("Index", homeView);
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult PurchaseInstruction()
        {
            ViewBag.Message = "Hướng dẫn cách mua hàng";

            var content = "";
            var header = "";

            using (var context = new ShoppingCartContext())
            {
                var contentObj = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.Purchase.Content");
                if (contentObj != null)
                {
                    content = contentObj.TextValue;
                }
                var headerObj = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.Purchase.Header");
                if (headerObj != null)
                {
                    header = headerObj.TextValue;
                }

            }

            var ret = new PurchaseInstruction()
            {
                Content = content,
                Header = header
            };
            return View(ret);
        }


        public ActionResult SearchProduct(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {

                //if (code == "All")
                //    return RedirectToAction("Index");
                var prods = new List<Product>();

                using (var context = new ShoppingCartContext())
                {
                    prods = context.Products.Where(p => p.Code == model.SearchKey.Trim()).ToList();
                    if (prods.Count == 0)
                        prods = context.Products.Where(p => p.Description.Contains(model.SearchKey.Trim())).Take(20).ToList();
                }
                using (var context = new ShoppingCartContext())
                {
                    var products = new List<ProductViewModel>();
                    var offers = context.ProductOffers.ToList();
                    foreach (var p in prods)
                    {
                        products.Add(new ProductViewModel()
                        {
                            Id = p.Id,
                            Code = p.Code,
                            Description = p.Description,
                            Price = offers.Single(po => po.ProductId == p.Id && po.PriceTypeId == 1).Price,
                            FeatureProduct = p.FeatureProduct,
                            Image = p.Image
                        });
                    }

                    foreach (var prod in products)
                    {
                        var origPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 3);
                        prod.OriginalPrice = origPrice != null ? origPrice.Price : 0m;
                    }

                    var homeView = new HomeViewModel { FilteredProducts = products };
                    //homeView.SelectedCategory = ;

                    //var ret = new HeaderAdvertisementViewModel();
                    //var id = context.ContentTypes.Single(p => p.Code == "Ad").Id;

                    //context.Contents.Where(c => c.ContentTypeId == id).ForEach(c => ret.Ads.Add(new HeaderAd()
                    //{
                    //    Image = c.Image,
                    //    Url = c.ImageUrl,
                    //    AdText = c.AdText,
                    //    AdTextStyle = c.AdTextStyle
                    //}));
                    //ret.Ads = ret.Ads.OrderBy(o => o.DisplayOrder).ToList();

                    //homeView.Advertisement = ret;
                    homeView.Advertisement = new HeaderAdvertisementViewModel()
                    {
                        Ads = new List<HeaderAd>()
                    };

                    var contactInfo = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.ContactInfo");

                    if (contactInfo != null)
                    {
                        homeView.Info.ContactInfo = contactInfo.TextValue;
                    }

                    var annoucement = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.Announcement");
                    if (annoucement != null)
                    {
                        homeView.Info.Annoucment = annoucement.TextValue;
                    }

                    ViewBag.ProductHeader = string.Empty;
                    return View("Index", homeView);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}