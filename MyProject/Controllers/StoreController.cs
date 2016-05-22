using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class StoreController : Controller
    {
        //
        // GET: /Store/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Details(int id)
        {
            var productModel = new ProductDetailViewModel();

            using (var pContext = new ShoppingCartContext())
            {
                var shippingRate = Convert.ToDecimal(pContext.AppSettings.Single(a => a.Code == "ShippingRate").Value);


                var prod = pContext.Products.Single(p => p.Id == id);
                var offers = pContext.ProductOffers.ToList();

                productModel.Code = prod.Code;
                productModel.Id = prod.Id;
                productModel.Description = prod.Description;
                productModel.ReviewVideoUrl = prod.ReviewVideoUrl;
                var origPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 3);
                var curPrice = offers.SingleOrDefault(po => po.ProductId == prod.Id && po.PriceTypeId == 1);

                var cat = pContext.Products.Where(p => p.Id == prod.Id).SelectMany(product => product.Categories).FirstOrDefault();
                if(cat != null)
                {
                    productModel.Category = cat;
                }

                if (origPrice != null)
                {
                    if (origPrice.Price > 0)
                    {
                        productModel.OriginalPrice = origPrice.Price;
                    }
                }
                if (curPrice != null)
                {
                    productModel.Price = curPrice.Price;
                }
                //productModel.Price = offers.Single(po => po.ProductId == prod.Id && po.PriceTypeId == 1).Price;
                productModel.Image = prod.Image;
                productModel.ImageAlt0 = prod.ImageAlt0;
                productModel.ImageAlt1 = prod.ImageAlt1;
                productModel.DetailDescription = prod.DetailDescription;
                productModel.ShippingCost = prod.Weight*shippingRate;
            }
            return View("Detail", productModel);
        }

        //public ActionResult ContinueToCart(int id)
        //{
        //    var prod = _shoppingCartContext.Products.SingleOrDefault(p => p.Id == id);
        //    return RedirectToAction("AddToCart", "ShoppingCart", prod);
        //}
	}
}