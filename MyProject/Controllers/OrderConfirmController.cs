using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class OrderConfirmController : Controller
    {
        public ActionResult Index()
        {
            OrderConfirmViewModel model = (OrderConfirmViewModel) TempData["OrderInfo"];
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(OrderConfirmViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    FullName = model.CheckOutInfo.Name,
                    Address = model.CheckOutInfo.ShippingAddress,
                    PaymentTransaction = model.CheckOutInfo.PaymentTransaction,
                    Email = model.CheckOutInfo.Email,
                    OrderDate = DateTime.Now,                

                };
                ShoppingCart.GetCart(this).CreateOrder(order);
            }
            return View();
        }
    }
}