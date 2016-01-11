using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

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
                var prods = pContext.Products.Where(p => p.FeatureProduct == true);
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
            }

            homeView.ProductViewModels = products;
            return View(homeView);
        }

        public ActionResult FilterByCategory(string code, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var prods = new List<Product>();

                using (var context = new ShoppingCartContext())
                {
                    prods = (from p in context.Products
                        where p.Categories.Any(c => c.Code == code)
                        select p).Take(24).ToList();
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
                    var homeView = new HomeViewModel {ProductViewModels = products};
                    homeView.SelectedCategory = code;

                    return View("Index", homeView);
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}