using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace MyProject.Controllers
{
    public class StoreController : Controller
    {
        ShoppingCartContext _shoppingCartContext = new ShoppingCartContext();
        //
        // GET: /Store/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Details(int id)
        {
            var prod = _shoppingCartContext.Products.SingleOrDefault(p => p.Id == id);
            return View("ProductDetails", prod);
        }

        public ActionResult ContinueToCart(int id)
        {
            var prod = _shoppingCartContext.Products.SingleOrDefault(p => p.Id == id);
            return RedirectToAction("AddToCart", "ShoppingCart", prod);
        }
	}
}