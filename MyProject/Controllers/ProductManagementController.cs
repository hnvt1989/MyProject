using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class ProductManagementController : Controller
    {
        //
        // GET: /ProductManagement/
        public ActionResult Index()
        {
            

            using (var context = new ShoppingCartContext())
            {
                var productCategories = new ProductManagementViewModel();

                var categories = context.Categories.ToList();
                var defaultCat = categories.First();

                var prods = from p in context.Products
                                    where p.Categories.Any(c => c.Code == defaultCat.Code)
                                    select p;
                //productCategories.Products.AddRange(prods);
                //                  where cartItems.Code == ShoppingCartId
                foreach (var prod in prods)
                {

                    productCategories.Products.Add(new ProductViewModel()
                    {
                        Code = prod.Code,
                        Description = prod.Description,
                        Image = prod.Image
                    });
                }
                return View(productCategories);
            }
        }

        public ActionResult Index1()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult AddNew()
        {
            return View();
        }
	}
}