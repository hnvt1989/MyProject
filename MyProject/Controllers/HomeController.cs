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
            return View(products);
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