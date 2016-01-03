using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.BuilderProperties;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using Address = MyProject.Models.ShoppingCart.Address;

namespace MyProject.Controllers
{
    public class CheckoutController : Controller
    {
        //
        // GET: /Checkout/
        public ActionResult Index(decimal cartTotal)
        {
            var checkoutModel = new CheckoutViewModel()
            {
                CartTotal = cartTotal,
                PaymentAmount = cartTotal,
                PaymentTransaction = new PaymentTransaction()
                {
                    //todo return billing address from account
                    BillingAddress = null,
                    PaymentType = null
                },
                //todo: return shipping address from account
                ShippingAddress = null,
            };
            return View(checkoutModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel model)
        {
            return View();
        }
	}
}