using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models;
using MyProject.Models.ShoppingCart;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (var pContext = new ShoppingCartContext())
            {
                var prods = pContext.Products.Where(p => p.FeatureProduct == true);
                products.AddRange(prods);

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