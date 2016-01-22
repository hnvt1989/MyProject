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

            List<ProductViewModel> products = new List<ProductViewModel>();

            using (var pContext = new ShoppingCartContext())
            {
                var prods = pContext.Products.Where(p => p.FeatureProduct && p.Active);
                var offers = pContext.ProductOffers.ToList();
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

                homeView.Advertisement = ret;

            }

            homeView.ProductViewModels = products;
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
                        select p).Where(p=> p.Active).Take(48).ToList();
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

                    var homeView = new HomeViewModel {ProductViewModels = products};
                    homeView.SelectedCategory = code;

                    var ret = new HeaderAdvertisementViewModel();
                    var id = context.ContentTypes.Single(p => p.Code == "Ad").Id;

                    context.Contents.Where(c => c.ContentTypeId == id).ForEach(c => ret.Ads.Add(new HeaderAd()
                    {
                        Image = c.Image,
                        Url = c.ImageUrl,
                        AdText = c.AdText,
                        AdTextStyle = c.AdTextStyle
                    }));
                    ret.Ads = ret.Ads.OrderBy(o => o.DisplayOrder).ToList();

                    homeView.Advertisement = ret;

                    var contactInfo = context.Contents.SingleOrDefault(c => c.TextLocation == "Home.ContactInfo");

                    if (contactInfo != null)
                    {
                        homeView.Info.ContactInfo = contactInfo.TextValue;
                    }

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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}